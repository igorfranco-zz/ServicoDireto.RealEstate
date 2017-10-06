var datetime = require('node-datetime');
var zap_crawler = require('./zap_crawler');
var zap_tools  = require('./zap_tools');
var zap_mongo  = require('./zap_mongo');
const EventEmitter = require('events');
class ImportEventHandler extends EventEmitter {}
var importEventHandler = new ImportEventHandler();
const BLOCK_SIZE = 10000;
var args = process.argv.slice(2);
//
importEventHandler.on('starting', ()=>{
    console.log('Starting proccess');
    console.log('Arguments:') 
    console.log(args);
});
//      
importEventHandler.on('status', (message)=>{
    console.log(message);
});

// 
var _config = {
    TipoOferta:1, //venda 1-venda 2-aluguel 3-lancamento    
    Pagina:1,
    Ordenacao: "",
    //PathName: '/venda/imoveis/rs+porto-alegre/',
    PathName: "",
    Bairro:"",
    Zona:"",
    //Cidade:"Porto Alegre",
    Cidade:"",
    TotalPaginas : 0
};
importEventHandler.emit('starting');
//
if((args == null || args.length == 0)||(args.length == 1))
{
    if(args.length == 1)
        _config.Pagina = parseInt(args[0]);

    importEventHandler.emit('status', 'Getting total pages');
    zap_crawler.GetTotalPages(_config, function(total){
        importEventHandler.emit('status', 'total pages: '+ total);
        var totalBlock = parseInt(total / BLOCK_SIZE);
        if(total % BLOCK_SIZE > 0)
            totalBlock++;

        importEventHandler.emit('status', 'total blocks: ' + totalBlock);
        zap_mongo.InsertUpdateExecution({IDCustomer: 'ZAP', ExecutionDate:datetime.create(), TotalPages: total, BlockSize: BLOCK_SIZE, Config: _config}, (result)=>{
            importEventHandler.emit('status', 'Execution OK 1');
        });
        for (var index = 1; index <= totalBlock; index++) {
            var config = JSON.parse(JSON.stringify(_config)); //cloning
            config.Pagina = (BLOCK_SIZE * index) - BLOCK_SIZE;
            if(config.Pagina == 0)
                config.Pagina = 1;

            config.TotalPaginas = BLOCK_SIZE * index;
            importEventHandler.emit('status', 'Processsing page: '  + config.Pagina);
            zap_tools.ProcessPage(config.Pagina, config, processStatus);
        }                    
    });
}
else if(args.length == 2) //inicio e fim
{
    _config.Pagina        =  parseInt(args[0]);
    _config.TotalPaginas  =  parseInt(args[1]);
    importEventHandler.emit('status', 'Processsing page: '  + _config.Pagina);
    zap_mongo.InsertUpdateExecution({IDCustomer: 'ZAP', ExecutionDate:datetime.create(), Config: _config}, (result)=>{
        importEventHandler.emit('status', 'Execution OK 2');
    });    
    zap_tools.ProcessPage(_config.Pagina, _config, processStatus);     
}
//
var processStatus = function(result)
{
    importEventHandler.emit('status', "status:" + result.status + ' page: ' + result.config.Pagina);
}



