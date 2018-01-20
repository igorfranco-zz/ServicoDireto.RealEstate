
	/**
	 * Buscar o corpo das paginas e processar
	 */
	var request = require('request');
	var zap_mongo = require('./zap_mongo');
	var zap_tools = require('./zap_tools');
	var cheerio = require('cheerio');
	var process = require('process'); 
	
	/**
	 * criadas configuracoes de request de acordo com o informado
	 */
	var createOpts = function(config)
	{
		var opts = 
		{
			url: 'https://www.zapimoveis.com.br/Busca/RetornarBuscaAssincrona/',
			method: 'POST',
			headers: {
				'Accept': '*/*',
				'X-Requested-With': 'XMLHttpRequest',
				'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
				'Host': 'www.zapimoveis.com.br'
			},
			form:{
				tipoOferta: config.TipoOferta, //1,
				paginaAtual: config.Pagina,
				ordenacaoSelecionada: config.Ordenacao,
				pathName: config.PathName, //'/venda/imoveis/rs+porto-alegre/' ,
				hashFragment: JSON.stringify({"precomaximo":"2147483647","parametrosautosuggest":[{"Bairro":config.Bairro,"Zona":config.Zona,"Cidade":config.Cidade,"Agrupamento":"","Estado":config.Estado}],"pagina":config.Pagina,"ordem":config.Ordenacao,"paginaOrigem":"ResultadoBusca","semente":"271261757","formato":"Lista"}),
				formato: 'Lista' 
			}		
		};

		return opts;
	};

	/**
	 * Busca o total de paginas a serem processadas
	 */
	exports.GetTotalPages = function (config, callback) 
	{
		var opts = createOpts(config);
		request(opts, function optionalCallback(err, response, body) 
		{
			if (err) { 
				//return
				console.error(' failed:', err);
				console.log("_______retry gettotalpages");
				exports.GetTotalPages(config, callback);
			}
			else
			{
				var record = JSON.parse(body); 
				callback(record.Resultado.QuantidadePaginas);
			}
		});			
	}
	/**
	 * Busca o conteudo da pagina principal, conforme o config informado
	 */
	exports.GetPageContent = function (config, callback) 
	{
		console.log("******************************** Processando página: " + config.Pagina + ' de ' + config.TotalPaginas + '********************************');			
		var opts = createOpts(config);
		request(opts, function optionalCallback(err, response, body) 
		{
			if (err) {
				console.error(' failed:', err);
				console.log("_______retry execute");
				callback({ status:'error', pid: process.pid }, err);
			}
			
			try 
			{
				var records = JSON.parse(body); 
				zap_mongo.InsertCollection(records.Resultado.Resultado, function(result){
					callback( { status: 'success', config: config , pid: process.pid  } );
				});	
			} catch (error) {
				callback({ status: 'error' , pid: process.pid }, error);
			}
		});						
	}
	/**
	 * Busca o detalhe do imovel
	 */
	exports.GetPageItemContent = function (baseUrl, callback) 
	{
		var opts = 
		{
			url: baseUrl,
			method: 'GET',
			headers: {
				'Accept': '*/*',
				'X-Requested-With': 'XMLHttpRequest',
				'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
				'Host': 'www.zapimoveis.com.br'
			}	
		};
		request(opts, function(err, response, body) 
		{
			if (err) { 
				console.error(' failed:', err);
				console.log("_______retry ImportItem");
				callback({ status: 'error' , pid: process.pid }, err);
			}
			else
			{
				const $ = cheerio.load(response.body); 
				$("strong").remove();
				
				callback({ 	
					id: $("#ofertaId ").data("value"),
					url: baseUrl, 
					description: $("#descricaoOferta p").html(),
					realstate_base: $("#caracteristicaOferta p").eq(0).html(),
					realstate_info: $("#caracteristicaOferta p").eq(1).html(), //Caracter&#xED;sticas do Im&#xF3;vel
					realstate_areas: ($("#caracteristicaOferta p").eq(2).html() + $(".content-ficha .outras-infos p").html()) //Caracter&#xED;sticas das &#xC1;reas Comuns
				} );
			}
		});			
	}