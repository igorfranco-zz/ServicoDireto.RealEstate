var zap_mongo  = require('./zap_mongo');
var request = require('request');
//const $ = require('cheerio');
const cheerio = require('cheerio');

//
var createOpts = function(baseUrl)
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

    return opts;
};

exports.ImportItem = function (baseUrl, callback) 
{
    var opts = createOpts(baseUrl);
    request(opts, function optionalCallback(err, response, body) 
    {
        if (err) { 
            //return
            console.error(' failed:', err);
            console.log("_______retry ImportItem");
        }
        else
        {
            const $ = cheerio.load(result.body); 
            callback({ 
                id: $("#ofertaId ").data("value"),
                url: baseUrl, 
                body: body, 
                description: $("#descricaoOferta p").html(),
                realstate_base: $("#caracteristicaOferta p").eq(0).html(),
                realstate_info: $("#caracteristicaOferta p").eq(1).html(), //Caracter&#xED;sticas do Im&#xF3;vel
                realtate_areas: $("#caracteristicaOferta p").eq(2).html() //Caracter&#xED;sticas das &#xC1;reas Comuns
            } );
        }
    });			
}
//
exports.ImportItem('https://www.zapimoveis.com.br/oferta/venda+apartamento+3-quartos+centro+diadema+sp+56m2+RS275000/ID-10733838/?paginaoferta=7', function(result) {
    console.log(result.url);
    //console.log(result.body);
    
});
