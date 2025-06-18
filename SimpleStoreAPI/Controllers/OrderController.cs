using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleStoreAPI.DTOs;
using SimpleStoreAPI.Interfaces;

namespace SimpleStoreAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponceDto>>> GetOrders()
        {
            var result = await _orderService.GetUserOrdersAsync();

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponceDto>> GetOrder(string id)
        {
            var result = await _orderService.GetByIdAsync(id);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponceDto>> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            var result = await _orderService.CreateOrderAsync(createOrderDto);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return CreatedAtAction(nameof(GetOrder), new
            {
                id = result.Data!.Id
            }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderResponceDto>> UpdateOrderAsync(string id, UpdateOrderDto updateOrderDto)
        {
            var result = await _orderService.UpdateOrderAsync(id, updateOrderDto);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Data);
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult<OrderResponceDto>> UpdateOrderStatusAsync(string id,
            UpdateOrderStatusDto updateStatusDto)
        {
            var result = await _orderService.UpdateOrderStatusAsync(id, updateStatusDto);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Data);
        }

        [HttpPut("{id}/cancel")]
        public async Task<ActionResult> CancelOrderAsync(string id)
        {
            var result = await _orderService.CancelOrderAsync(id);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Order cancelled successfully");
        }
    }
}
