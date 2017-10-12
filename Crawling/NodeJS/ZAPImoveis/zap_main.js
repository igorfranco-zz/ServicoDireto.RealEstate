var datetime = require('node-datetime');
var crawler = require('./zap_crawler');
var tools  = require('./zap_tools');
var mongo  = require('./zap_mongo');

const BLOCK_SIZE = 10000;
var args = process.argv.slice(2);

// 
var _config = {
    TipoOferta: 1, //venda 1-venda 2-aluguel 3-lancamento    
    Pagina: 1,
    Ordenacao: "",
    PathName: "",
    Bairro: "",
    Zona: "",
    Cidade: "",
    TotalPaginas : 0
};
//
var _processPage = function(pageIndex, config, callback){    
    config.Pagina = pageIndex;
    crawler.GetPageContent(config, function(result, err){
        console.log(result.status + " --> " + result.config.Pagina);
        if(err != null)
            console.log(err)
        else    
            pageIndex++;

        if(pageIndex <= config.TotalPaginas)
        {
            _processPage(pageIndex, config, callback);                        
        }
    });    
};    

//mode: import, crawler_page, crawler_range

var def_args = { mode: 'import' };
//if(args.length > 0)
 //   def_args = JSON.parse(args[0]);

//
if(def_args.mode == 'crawler_page')
{
    _config.Pagina = def_args.start;

    crawler.GetTotalPages( _config, function( total ){
        var totalBlock = parseInt( total / BLOCK_SIZE );
        if(total % BLOCK_SIZE > 0)
            totalBlock++;

        for (var index = 1; index <= totalBlock; index++) {
            var config = JSON.parse(JSON.stringify(_config)); //cloning
            config.Pagina = (BLOCK_SIZE * index) - BLOCK_SIZE;
            if(config.Pagina == 0)
                config.Pagina = 1;

            config.TotalPaginas = BLOCK_SIZE * index;
            _processPage(config.Pagina, config, (response, err) => {
                console.log(response);
                console.error(err);
            });
        }                    
    });
}
else if(def_args.mode == 'crawler_range')
{
    console.log("----> Inicio e fim pre-definido");
    _config.Pagina        =  def_args.start;
    _config.TotalPaginas  =  def_args.end;

    _processPage(_config.Pagina, _config, (response, err) => {
        console.log(response);
        //console.error(err);
    });
}
else if(def_args.mode == 'import')
{
    console.log("----> importacao");
    mongo.GetUnproceededAndSaveToDB(function(result, err){
        if(err!=null)
            console.log(err);
            
        console.log(result)
    });
}