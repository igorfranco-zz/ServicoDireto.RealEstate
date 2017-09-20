//	process.env['NODE_TLS_REJECT_UNAUTHORIZED'] = '0'; // Ignore 'UNABLE_TO_VERIFY_LEAF_SIGNATURE' authorization error
	var request = require('request');
	var zap_mongo = require('./zap_mongo');
	var zap_tools = require('./zap_tools');
	var fs = require('fs');
	var mkdirp = require('mkdirp');
//     
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
//
	exports.Import = function (config, callback) 
	{
		console.log("******************************** Processando página: " + config.Pagina + ' de ' + config.TotalPaginas + '********************************');			
		var opts = createOpts(config);
		//console.log(opts.form);
		request(opts, function optionalCallback(err, response, body) 
		{
			if (err) {
				//return
				console.error(' failed:', err);
				console.log("_______retry execute");
				callback({status:'error'}, err);
			}
			
			try 
			{
				var records = JSON.parse(body); 	
				zap_mongo.InsertUpdate(records.Resultado.Resultado, function(result){
					console.log(result);
					callback({status:'success'});
				});				
			} catch (error) {
				callback({status:'error'}, error);
			}
		});				
		
		/*
		var opts = createOpts(config);
		request(opts, function optionalCallback(err, response, body) 
		{
			if (err) {
				return console.error('failed:', err);
			}
			var record = JSON.parse(body); 
			execute(config, function(record){
				callback(record);
			});
		});
		*/			
	}
//
	var downloadPictures = function(element, callback){
		var basePath = './zap/' + element.CodigoOfertaZAP;
		/*
		mkdirp(basePath, function (err) {
			if (err) {
				console.error(err)
			}
			else 
			{
				for (var index = 0; index < element.Fotos.length; index++) {
					var photo = element.Fotos[index];
					var path = basePath + '\\' + element.CodigoOfertaZAP + '.jpg';
					zap_tools.Download(photo.UrlImagem, path, function(){
						console.log('Image OK: ' + path);
						callback({status:'concluded', element:element, path: path});
					});
				}		
			}
		});		
		*/
	};

	//
	var createOpts = function(config)
	{
		var opts = 
		{
			url: 'https://www.zapimoveis.com.br/Busca/RetornarBuscaAssincrona/',
			//proxy: "http://127.0.0.1:8888", // Note the fully-qualified path to Fiddler proxy. No "https" is required, even for https connections to outside.
			//proxy:'http://94.177.180.226:80',//OK
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