using BookStore.Application.Book.Command;
using BookStore.Application.Book.Model;
using BookStore.Application.Book.SearchBook;
using BookStore.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers;

public class LibraryController : ApiControllerBase
{
  
    /// <summary>
    /// Search for books  
    /// </summary>
    /// <param name="query"></param>
    /// <remarks>
    ///
    /// </remarks>
    /// <returns></returns>
    [HttpGet("books")]
    [SwaggerResponse(StatusCodes.Status200OK, type:  typeof(Result<PaginatedList<GetBookDto>>))]
    public async Task<IActionResult> SearchBook([FromQuery] SearchBookQuery query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
    
    /// <summary>
    ///  Endpoint to make reservation
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpPost("make-reservation")]
    [SwaggerResponse(StatusCodes.Status200OK, type:  typeof(Result))]
    public async Task<IActionResult> Reserve([FromBody] ReserveBookCommand query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
    
       
    /// <summary>
    ///  endpoint to cancel reservation
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpPost("cancel-reservation")]
    [SwaggerResponse(StatusCodes.Status200OK, type:  typeof(Result))]
    public async Task<IActionResult> CancelReservation([FromBody] CancelReserveCommand query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
    
    /// <summary>
    ///  endpoint to borrow book
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpPost("borrow")]
    [SwaggerResponse(StatusCodes.Status200OK, type:  typeof(Result))]
    public async Task<IActionResult> BorrowBook([FromBody] BorrowBookCommand query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
    
    
    
    /// <summary>
    ///  endpoint to return book
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpPost("return")]
    [SwaggerResponse(StatusCodes.Status200OK, type:  typeof(Result))]
    public async Task<IActionResult> BorrowBook([FromBody] ReturnBookCommand query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
 
    
    
    /// <summary>
    ///  endpoint to Request for Book When its available
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpPost("request-book")]
    [SwaggerResponse(StatusCodes.Status200OK, type:  typeof(Result))]
    public async Task<IActionResult> RequestBookWhenAvaliable([FromBody] RequestBookWhenAvaliableCommand query)
    {
        var result = await Mediator.Send(query);
        return Ok(result);
    }
}