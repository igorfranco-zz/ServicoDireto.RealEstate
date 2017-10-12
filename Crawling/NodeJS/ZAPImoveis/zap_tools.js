var zap_crawler = require('./zap_crawler');
var request = require('request');
var fs = require('fs');

/**
 * Efetua o download de uma arquivo na web
 */
exports.Download = function(uri, filename, callback){
		request.head(uri, function(err, res, body){
			console.log('content-type:', res.headers['content-type']);
			console.log('content-length:', res.headers['content-length']);
			request(uri).pipe(fs.createWriteStream(filename)).on('close', callback);
		});
    };

/**
 * regex que traz apenas numeros
 *  */
exports.GetOnlyNumber = function(value)
{  
    var numberPattern = /\d+/g;     
    if(value == null)
        value = '';

    var result =  value.match( numberPattern );
    if (result != null && result.length > 0)
        return result.join('');
    else 
        return null;
}

/**
 * Buscar os atributos de uma linha de detalhe
 * @param {*} record 
 * @param {*} group 
 */
exports.GetAttributes = function(record, group, result)
{
    if(record != null)
    {
        record = record.toString().replace("null", "").trim();
        record.split(',').forEach(function(element) {
            result.push( { name:element, value: "" , group: group } );	
        }, this);
    }
    //return result;
}