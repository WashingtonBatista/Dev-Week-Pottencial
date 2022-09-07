using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



using Dev_Week_Pottencial.src.Models;
using Dev_Week_Pottencial.src.Persistence;


namespace Dev_Week_Pottencial.src.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private DatabaseContext _context { get; set; }

        public PersonController(DatabaseContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public ActionResult<List<Pessoa>> Get(){
            
            var result = _context.Pessoas.Include( p => p.contratos).ToList();

            if (!result.Any()) return NoContent();

            return Ok(result);
        }

        [HttpPost]

        public ActionResult<Pessoa> Post([FromBody]Pessoa pessoa)
        {
            try
            {
              _context.Pessoas.Add(pessoa);
              _context.SaveChanges();  
            }
            catch (System.Exception)
            {
             return BadRequest();
            }
            return Created("criado", pessoa);
              
        }

        [HttpPut("{id}")]

        public ActionResult<Object> Update([FromRoute]int id, [FromBody]Pessoa pessoa)
        {
           var result = _context.Pessoas.SingleOrDefault(e => e.Id == id);
           
           if (result is null){
            return NotFound(new {
                msg = "Registro não encontrado",
                status = HttpStatusCode.NotFound
            });
           }

           try
           {
            _context.Pessoas.Update(pessoa);
            _context.SaveChanges();
           }
           catch (System.Exception)
           {
            return BadRequest(new {
                msg = $"Houve erro ao enviar solicitação de atualização {id} atualizados",
                status = HttpStatusCode.OK
            });
           }
           
            
            
            return Ok( new {
                msg = $"Dados do id {id} atualizados",
                status = HttpStatusCode.OK
            });
        }

        [HttpDelete("{id}")]

        public ActionResult<Object> Delete([FromRoute]int id)
        {
            var result = _context.Pessoas.SingleOrDefault(e => e.Id == id);

            if (result is null){
                return BadRequest(new {
                    msg = "Conteúdo inexistente, solicitação inválida",
                    status = HttpStatusCode.BadRequest
                });
            } 
            
            _context.Pessoas.Remove(result);
            _context.SaveChanges();
            
            return Ok(new {
                msg = "deletado pessoa de Id " + id,
                status = HttpStatusCode.OK
            });

        }
        
        

    }   
}







    



    
