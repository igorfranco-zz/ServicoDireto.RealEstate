var zap_crawler = require('./zap_crawler');
var request = require('request');
var fs = require('fs');
//
exports.Download = function(uri, filename, callback){
		request.head(uri, function(err, res, body){
			console.log('content-type:', res.headers['content-type']);
			console.log('content-length:', res.headers['content-length']);
			request(uri).pipe(fs.createWriteStream(filename)).on('close', callback);
		});
    };	
//    
exports.ProcessPage = function(pageIndex, config, callback){    
    config.Pagina = pageIndex;
    zap_crawler.Import(config, function(result){
        if(result.status == "success")
            pageIndex++;

        console.log(result);
        if(pageIndex <= config.TotalPaginas)
        {
            //callback({status:'executing', config})
            exports.ProcessPage(pageIndex, config, callback);                        
        }
    });    
};    