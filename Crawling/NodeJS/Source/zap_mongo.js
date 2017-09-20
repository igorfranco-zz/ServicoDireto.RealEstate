var MongoClient = require('mongodb').MongoClient;
var assert = require('assert')
var request = require('request')
var datetime = require('node-datetime');
var conn = MongoClient.connect('mongodb://localhost:27017/crawler'); // returns a Promise

var execution = 
{
    IDCustomer: 'ZAP',
    ExecutionDate:'',
    TotalPages: 0,
    BlockSize: 0
}

var executionItem = 
{
    IDCustomer: 'ZAP',
    RetryCounter: 0,     
    Config: null,
}

exports.ExportToDB = function(callback){
    conn.then(db => db.collection('zap').find({ dt_proceeded: { $exists: false }}).limit(500).toArray(function(err, result) {	
        result.forEach(function(element) {
            console.log("-----Processando " + element.CodigoOfertaZAP);
            var record = {
                id : /*"ZAP_" + */element.CodigoOfertaZAP,
                defaultPicture : element.UrlFotoDestaqueTamanhoP,
                url : element.UrlFicha,
                title : element.TituloPagina,
                description : element.Observacao,
                cityName : element.CidadeOficial,
                countryName : "pt-BR" ,
                stateProvinceName : element.Estado,
                neighborhood : element.BairroOficial,
                purposeName : element.Transacao,
                categoryName : element.SubTipoOferta,
                typeName : element.SubTipoOferta,
                postalCode : element.CEP,
                address : element.Endereco.length == 0 ? "-" : element.Endereco,
                latitude : element.Latitude,
                longitude : element.Longitude,
                customer: 
                {
                        id: "ZAP_" + element.CodigoAnunciante,
                        name: element.NomeAnunciante, 
                        logo: element.UrlLogotipoCliente, 
                        phone: (element.ContatoCampanha != null && element.ContatoCampanha.Telefone != null) ? "(" + element.ContatoCampanha.Telefone.DDD + ")" + element.ContatoCampanha.Telefone.Numero : "-",
                        email: (element.ContatoCampanha != null && element.ContatoCampanha.Email != null) ? element.ContatoCampanha.Email : "-"
                },
                attributes: 
                [
                    { name:"Preco", value:element.PrecoVendaMaximo },                    
                    { name:"PrecoVendaMaximo", value:element.PrecoVendaMaximo },
                    { name:"PrecoVendaMinimo", value:element.PrecoVendaMinimo },

                    { name:"QuantidadeQuartos", value:element.QuantidadeQuartos },
                    { name:"QuantidadeQuartosMinima", value:element.QuantidadeQuartosMinima },
                    { name:"QuantidadeQuartosMaxima", value:element.QuantidadeQuartosMaxima },

                    { name:"QuantidadeVagas", value:element.QuantidadeVagas },
                    { name:"QuantidadeVagasMinima", value:element.QuantidadeVagasMinima },
                    { name:"QuantidadeVagasMaxima", value:element.QuantidadeVagasMaxima },

                    { name:"QuantidadeSuites", value:element.QuantidadeSuites },
                    { name:"QuantidadeSuitesMinima", value:element.QuantidadeSuitesMinima },
                    { name:"QuantidadeSuitesMaxima", value:element.QuantidadeSuitesMaxima },

                    { name:"Area", value:element.Area },
                    { name:"AreaMinima", value:element.AreaMinima },
                    { name:"AreaMaxima", value:element.AreaMaxima },

                    { name:"PrecoCondominio", value:element.PrecoCondominio },
                    { name:"ValorIPTU", value:element.ValorIPTU }
                ],
                images: []
            };
            //loading images
            element.Fotos.forEach(function(image) {
                record.images.push(
                    { 
                        urlImageMain: image.UrlImagem, 
                        urlImageSize1: image.UrlImagemTamanhoPP,
                        urlImageSize2: image.UrlImagemTamanhoP,
                        urlImageSize3: image.UrlImagemTamanhoM,
                        urlImageSize4: image.UrlImagemTamanhoG,
                        isMain: image.Principal,
                    });
            }, this);
            //
             var opts = 
            {
                url: 'http://localhost:8082/api/apielement/Import',
                method: 'POST',
                json: record,
                headers: {
                    "content-type": "application/json",
                }
            };
            //
            request(opts, function optionalCallback(err, response, record)
            {
                if (err) {
                    console.error(err);
                }
                else
                {
                    //console.log(record);
                    try {
                        conn.then(db => db.collection('zap')
                            .findOneAndUpdate({ CodigoOfertaZAP: record.id }, {$set: { "dt_proceeded" : datetime.create() }}));                        
                    }
                    catch(e){
                        console.log(e);
                    }

                    callback(record);
                }
            });				  
        }, this);
        //console.log(result)
    }));   
}

exports.InsertUpdateExecution = function (record, callback) 
{
    conn.then(db => db.collection('execution').count({ IDCustomer: record.IDCustomer }, function(err, result) {	
        if(result == 0)
        {
            console.log("execution does not exists");
            conn.then(db => db.collection('execution').insert(record, (err, result) => {
                if(err != null)
                    console.error(' failed:', err);
                else
                    console.log("execution inserted");
                //db.close();
            }));
        }

        else
        {
            console.log("execution does exists");
            conn.then(db => db.collection('execution').update({IDCustomer: record.IDCustomer}, record, (err, result) => {
                if(err != null)
                    console.error(' failed:', err);
                else
                    console.log("execution updated");
                // db.close();
            }));                        
        }    
    }));
}
//
exports.InsertUpdate = function (records, callback) 
{
    conn.then(db => db.collection('zap').insertMany(records, function(err) {	
        callback({ status:'inserted', count: records.length});
    }));	    
};
//
exports.ExportToDB((result)=>{
    console.log(result) ;
});