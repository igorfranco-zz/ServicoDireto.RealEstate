var apiUrl = "http://192.168.0.6/servicodireto";
var mongoUrl = 'mongodb://192.168.0.6:27017/crawler';

var MongoClient = require('mongodb').MongoClient;
var ObjectID = require('mongodb').ObjectID;
var tools  = require('./zap_tools');
var crawler  = require('./zap_crawler');
var assert = require('assert')
var request = require('request')
var datetime = require('node-datetime');
var process = require('process'); 
var Fiber = require('fibers');


var conn = MongoClient.connect(mongoUrl); // returns a Promise
var _countProc = 0;

const TOTAL_BLOCK = 10 ;
const COLLECTION_NAME = "zap";

/**
 * Salvar registro como processado
 */
exports.SaveRecord = function(item, callback)
{
    //
    var opts = 
    {
        url: apiUrl + '/api/apielement/Import',
        method: 'POST',
        json: item,
        //pool: httpAgent,
        headers: {
            "content-type": "application/json",
        }
    };
    //
    request(opts, function optionalCallback(err, response, record)
    {
        if (err || record.stackTrace) {
            callback({status: 'error', result: record, pid: process.pid}, err);
        }
        else
        {
            try {
                conn.then(db => db.collection(COLLECTION_NAME)
                    .findOneAndUpdate({ _id: new ObjectID(record.idMongo) }, 
                                      { $set: { "_proceeded" : 1 } }, 
                                      {},
                                      (err, doc, raw) => {
                                        _countProc++;

                                        if(err != null)
                                            callback({status: 'error', pid: process.pid}, err);

                                        if(_countProc == TOTAL_BLOCK)
                                        {
                                            _countProc = 0;
                                            callback({status: 'success_block', pid: process.pid} );
                                        }
                                        else
                                            callback({status: 'success_item',result: record, pid: process.pid} );
                                    }));         
            }
            catch(e){
                callback({status: 'error', pid: process.pid}, e);
            }
        }
    });	
}

/**
 * Buscar registro nao processado e importa para plataforma
 */
exports.GetUnproceededAndSaveToDB = function( callback ){
    Fiber(function() {
        conn.then(db => db.collection(COLLECTION_NAME).find({ _proceeded: { $exists: false }}).limit( TOTAL_BLOCK ).toArray(function(err, result) {	        
            result.forEach(function(element) 
            {
                console.log("-----Processando " + element._id);
                var record = {
                    idMongo: element._id,
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
                        { name:"Preco", value: tools.GetOnlyNumber(element.Valor), group: "BASIC" },                    
                        { name:"PrecoVendaMaximo", value: (element.PrecoVendaMaximo), group: "BASIC"  },
                        { name:"PrecoVendaMinimo", value: (element.PrecoVendaMinimo), group: "BASIC"  },

                        { name:"QuantidadeQuartos", value:element.QuantidadeQuartos , group: "BASIC" },
                        { name:"QuantidadeQuartosMinima", value:element.QuantidadeQuartosMinima, group: "BASIC"  },
                        { name:"QuantidadeQuartosMaxima", value:element.QuantidadeQuartosMaxima, group: "BASIC"  },

                        { name:"QuantidadeVagas", value:element.QuantidadeVagas, group: "BASIC"  },
                        { name:"QuantidadeVagasMinima", value:element.QuantidadeVagasMinima, group: "BASIC"  },
                        { name:"QuantidadeVagasMaxima", value:element.QuantidadeVagasMaxima, group: "BASIC"  },

                        { name:"QuantidadeSuites", value:element.QuantidadeSuites, group: "BASIC"  },
                        { name:"QuantidadeSuitesMinima", value:element.QuantidadeSuitesMinima, group: "BASIC"  },
                        { name:"QuantidadeSuitesMaxima", value:element.QuantidadeSuitesMaxima, group: "BASIC"  },

                        { name:"Area", value:element.Area, group: "BASIC"  },
                        { name:"AreaMinima", value:element.AreaMinima, group: "BASIC"  },
                        { name:"AreaMaxima", value:element.AreaMaxima, group: "BASIC"  },

                        { name:"PrecoCondominio", value:element.PrecoCondominio, group: "BASIC"  },
                        { name:"ValorIPTU", value:element.ValorIPTU, group: "BASIC"  }
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
                
                //buscar os detalhes do imovel
                crawler.GetPageItemContent(record.url, (result, err) => {
                    if(err == null)
                    {
                        if(result.description!=null)
                            record.description = result.description; 

                        if(result.realstate_info != null)    
                            tools.GetAttributes(result.realstate_info, 'IE', record.attributes);

                        if(result.realstate_areas != null)
                            tools.GetAttributes(result.realstate_areas, 'IE', record.attributes);
                    }    

                    //salva igual porem sem detalhes
                    exports.SaveRecord(record, function(result, err){
                        callback(result, err);                
                    });
                })            			  
            }, this);
        }));        
    }).run();
}

/**
 * Insere um rgistro de execucao
 */
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

/**
 * Insere uma colecao de itens
 */
exports.InsertCollection = function (records, callback) 
{
    conn.then(db => db.collection(COLLECTION_NAME).insertMany(records, function(err) {	
        callback({ status:'inserted', count: records.length, pid: process.pid});
    }));	    
}

/**
 * Insere e atualiza um registro exclusivamente
 */
exports.InsertUpdateSingle = function (record, callback) 
{
    conn.then(db => db.collection(COLLECTION_NAME).insert(records, function(err) {	
        callback({ status:'inserted single', count: record.CodigoOfertaZAP, pid: process.pid});
    }));	    
}
