using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_API.Data;
using Project_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : Controller
    {
        private readonly CardsDbContext cardsDbContext;

        public CardsController(CardsDbContext cardsDbContext)
        {
            this.cardsDbContext = cardsDbContext;
        }

        //Get all card
        [HttpGet]
        [ActionName("GetAllCards")]
        public async Task<IActionResult> GetAllCards()
        {
            var cards = await cardsDbContext.Cards.ToListAsync();
            return Ok(cards);
        }

        //Get one card
        [HttpGet]
        [Route("{id:int}")]
        [ActionName("GetCard")]
        public async Task<IActionResult> GetCard([FromRoute] int id)
        {
            var cards = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (cards != null)
            {
                return Ok(cards);
            }
            return NotFound("Cards not found");
        }

        //Add card
        [HttpPost]
        public async Task<IActionResult> AddCard([FromBody] Card card)
        {
            Card newcard = new Card();
            newcard.CardholderName = card.CardholderName;
            newcard.CardNumber = card.CardNumber;
            newcard.ExpiryYear = card.ExpiryYear;
            newcard.ExpiryMonth = card.ExpiryMonth;
            newcard.CVC = card.CVC;
            await cardsDbContext.Cards.AddAsync(newcard);
            await cardsDbContext.SaveChangesAsync();
            return Ok(newcard);
        }

        //update
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateCard([FromRoute] int id, [FromBody] Card card)
        {
            var exitscard = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if(exitscard != null)
            {
                exitscard.CardholderName = card.CardholderName;
                exitscard.CardNumber = card.CardNumber;
                exitscard.ExpiryMonth = card.ExpiryMonth;
                exitscard.ExpiryYear = card.ExpiryYear;
                await cardsDbContext.SaveChangesAsync();
                return Ok(exitscard);
            }
            return NotFound("Card not found");

        }

        //delete
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteCard([FromRoute] int id)
        {
            var exitscard = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if(exitscard != null)
            {
                cardsDbContext.Remove(exitscard);
                await cardsDbContext.SaveChangesAsync();
                return Json(new { message = "CardId" + exitscard.Id + "delete"});
            }
            return NotFound("Card not found");
        }
    }
}
