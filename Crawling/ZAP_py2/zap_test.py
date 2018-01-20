import scrapy
import json
#data = json.dumps({"precomaximo":"2147483647","parametrosautosuggest":[{"Bairro":'',"Zona":'',"Cidade":'',"Agrupamento":"","Estado":''}],"pagina":'1',"ordem":'',"paginaOrigem":"ResultadoBusca","semente":"271261757","formato":"Lista"}, separators=(',',':'))
data = json.dumps ({"precomaximo":"2147483647","parametrosautosuggest":[{"Bairro":"","Zona":"","Cidade":"SAO PAULO","Agrupamento":"Zona Sul","Estado":"SP"}],"pagina":"1","ordem":"Relevancia","paginaOrigem":"ResultadoBusca","semente":"462818504","formato":"Lista"}, separators=(',',':'))
class ZAPTest(scrapy.Spider):
    name = "zap_test"

    def start_requests(self):
        urls = [
            'https://www.zapimoveis.com.br/Busca/RetornarBuscaAssincrona/'
        ]
        for url in urls:
            yield scrapy.FormRequest(url=url, callback=self.parse, method='POST',
			headers={
				'Accept': '*/*',
				'X-Requested-With': 'XMLHttpRequest',
				'Host': 'www.zapimoveis.com.br'
			},
            formdata={
				'tipoOferta': 'Imovel',
				'paginaAtual': '1',
				'ordenacaoSelecionada': '',
				'pathName': '/venda/apartamentos/agr+sp+sao-paulo+zona-sul/' ,
				'hashFragment': data,
				'formato': 'Lista' 
			})

    def parse(self, response):
        jsonresponse = json.loads(response.body_as_unicode())
        #item = MyItem()
        #item["firstName"] = jsonresponse["firstName"]   