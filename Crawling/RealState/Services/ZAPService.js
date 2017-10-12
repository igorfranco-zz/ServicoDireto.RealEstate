
/*
    Buscar total de paginas a patir da config informada
*/
exports.GetTotalPages = function (config, callback) 
{
    var opts = createOpts(config);
    request(opts,(err, response, body) => 
    {
        if (err) { 
            console.error(' failed:', err);
        }
        else
        {
            var record = JSON.parse(body); 
            callback(record.Resultado.QuantidadePaginas);
        }
    });			
}