using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers {
    [Route("[controller]")]
    [ApiController]

    //projeto funcionando

    public class ProdutosController : ControllerBase {//essa herança acontece para que essa api tenha algumas features a mais
    
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext context) {
            _context = context;
        }

        [HttpGet] //Get serve para consultar e retornar valores
        public ActionResult<IEnumerable<Produto>> Get() {//IEnumerable é usado pois há apenas uma interface de leitura, podendo trabalhar apenas sobre demanda.
            var produtos = _context.Produtos.ToList();
            if(produtos is null) {
                return NotFound("Produto não encontrado");
            }
            return produtos;
        }

        [HttpGet("{id:int}", Name ="ObterProduto")]//Consultando por id
        public ActionResult<Produto> Get(int id) {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);
            if (produto is null) {
                return NotFound("Produto não encontrado");
            }
            return produto;
        }

        [HttpPost]
        public ActionResult Post (Produto produto) {

            if (produto is null)
                return BadRequest();

            _context.Produtos.Add(produto);//inclui no contexto do banco de dados
            _context.SaveChanges();//Assim os dados são persistidos na tabela do SLQ

            return new CreatedAtRouteResult("ObterProduto", //retorna 201 created por causa do "CreatedAtRouteResult"
                new { id = produto.ProdutoId }, produto); //acionando a rota "ObterProduto" onde ele pega o id do produto e retorna os dados do produto
        }

        //para fazer atualização de um produto
        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto) {
            if (id != produto.ProdutoId) {
                return BadRequest();
            }
            _context.Entry(produto).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete ("{id:int}")]
        public ActionResult Delete(int id) {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);//nessa linha é onde encontra-se o produto dentro do BD

            if (produto is null) {
                return NotFound("Produto não localizado!");
            }
            _context.Produtos.Remove(produto);//Remove do banco de dados 
            _context.SaveChanges();//Persiste as alterações no banco de dados

            return Ok(produto);//Retorna as informações de 200, ou seja, OK
        }

    }
}
