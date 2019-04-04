using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittlePacktBookstore.Models;
using LittlePacktBookstore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LittlePacktBookstore.Controllers
{
	/// <summary>
	/// Book Controller.
	/// </summary>
	[Produces("application/json")]
	[ApiController]
	[Route("api/[Controller]")]
	public class BookController : Controller
	{
		private readonly IRepository<Book> _bookRepository;
		private readonly ILogger<BookController> _logger;

		//ask dependency injector to inject an instance of this class
		/// <summary>
		/// Constructor for the Book Controller.
		/// </summary>
		/// <param name="bookRepository"></param>
		/// <param name="logger"></param>
		public BookController(IRepository<Book> bookRepository, ILogger<BookController> logger)
		{
			_bookRepository = bookRepository;
			_logger = logger;
		}

		// GET: api/<controller>
		/// <summary>
		/// Returns all books.
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(200, Type =typeof(IEnumerable<Book>))]
		[ProducesResponseType(404)]
		//public ActionResult<IEnumerable<Book>> Get()
		public IActionResult Get()
		{
			try
			{
				return Ok(_bookRepository.GetAll());
			}
			catch(Exception ex)
			{
				_logger.LogError("Something went wrong: " + ex.Message);
				return NotFound();
			}
			
		}

		// GET api/<controller>/5
		/// <summary>
		/// Returns a book by index
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		public ActionResult<Book> Get(int id)
		{
			//return "value";
			return Ok(_bookRepository.Get(id));
		}

		// POST api/<controller>
		/// <summary>
		/// Add a new book.
		/// </summary>
		/// <param name="book"></param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult Post([FromBody]Book book)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					_logger.LogError("Invalid model state.");
					return BadRequest();
				}
				else
				{
					_bookRepository.Add(book);
					return Created($"/api/book/{book.Id}", book);
				}
			}
			catch(Exception ex)
			{
				_logger.LogError("Exception adding new book:  " + ex.Message);
				//throw;
				return BadRequest();	//returns 400 bad request
			}
		}

		// PUT api/<controller>/5
		/// <summary>
		/// Edit a book.
		/// </summary>
		/// <param name="book"></param>
		/// <returns></returns>
		[HttpPut]
		public IActionResult Put([FromBody]Book book)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					_logger.LogError("Invalid model state.");
					return BadRequest();
				}
				else
				{
					_bookRepository.Edit(book);
					return Ok(book);
				}
			}
			catch(Exception ex)
			{
				_logger.LogError("Exception editing book:  " + ex.Message);
				return BadRequest();
			}
		}

		// DELETE api/<controller>/5
		/// <summary>
		/// Delete a book.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			try
			{
				var book = _bookRepository.Get(id);
				if (book != null)
				{
					_bookRepository.Delete(book);
					return Ok("Book deleted.");
				}
				return BadRequest("Could not delete book.");
			}
			catch(Exception ex)
			{
				_logger.LogError("Exception deleting book:  " + ex.Message);
				return BadRequest();
			}
		}
	}
}
