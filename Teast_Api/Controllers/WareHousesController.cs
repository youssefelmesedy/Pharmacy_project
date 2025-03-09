using Microsoft.AspNetCore.Mvc;

namespace Teast_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WareHousesController : ControllerBase
    {
        private readonly WareHousesServices _services;
        private readonly IUnitOfWork _unitofwork;

        public WareHousesController(WareHousesServices services, IUnitOfWork unitofwork)
        {
            _services = services ?? throw new ArgumentNullException(nameof(_services));
            _unitofwork = unitofwork ?? throw new ArgumentNullException(nameof(_unitofwork));
        }

        // Get All WareHouses 
        [HttpGet("GetAllHouses")]
        public async Task<IActionResult> GetAll()
        {
            var houses = await _services.GetAllWareHouses();
            if (houses == null)
                return NotFound("❌ Not Found Houses In Database..!");

            return Ok(new
            {
                message = $" ✅ Get All Houses (❁´◡`❁) successfully.",
                GetAt = DateTime.UtcNow,
                Houses = houses
            });
        }

        // Get By Id WareHouses 
        [HttpGet("GetByIdHouses")]
        public async Task<IActionResult> GetById(int Id)
        {
            var houses = await _services.GetWareHouseById(Id);
            if (houses == null)
                return NotFound("❌ Not Found Houses In Database..!");

            return Ok(new
            {
                message = $" ✅ Get By Id: ({Id}) Houses (❁´◡`❁) successfully.",
                GetAt = DateTime.UtcNow,
                Houses = houses
            });
        }

        // Create Houses in DataBase
        [HttpPost("CreateHouses")]
        public async Task<IActionResult> Create(DtoWareHouses model)
        {
            var exist = await _unitofwork.Repository<Warehouse>().FindAsync(h => (h.Name.ToLower().Replace(" ", "") == model.Name.ToLower().Replace(" ", "")));
            if(exist != null)
            {
                return Conflict(new
                {
                    message = $" ⚠️ Added Houses Name: ({model.Name}) Arady Existing...! With Use Id: ({exist.Id}) .",
                    GetAt = DateTime.UtcNow,
                    Houses = exist
                });
            }
            var houses = await _services.CreateWareHouse(model);
            if (houses == null)
                return NotFound("❌ Not Found Houses In Database..!");

            return Ok(new
            {
                message = $" ✅ Added Houses Name: ({model.Name}) (❁´◡`❁) successfully.",
                GetAt = DateTime.UtcNow,
                Houses = houses
            });
        }

        // UpDate Houses in DataBase
        [HttpPut("UpDateHouses")]
        public async Task<IActionResult> Update(int id,DtoWareHouses model)
        {
            var exist = await _unitofwork.Repository<Warehouse>().FindAsync(h => (h.Name.ToLower().Replace(" ", "") == model.Name.ToLower().Replace(" ", "")) && h.Id != id);
            if (exist != null)
            {
                return Conflict(new
                {
                    message = $" ⚠️ Added Houses Name: ({model.Name}) Arady Existing...! With Use Id: ({exist.Id}) .",
                    GetAt = DateTime.UtcNow,
                    Houses = exist
                });
            }
            var houses = await _services.UpdateWareHouse(id,model);
            if (houses == null)
                return NotFound("❌ Not Found Houses In Database..!");

            return Ok(new
            {
                message = $" ✅ UpDate Houses Name: ({model.Name}) (❁´◡`❁) successfully.",
                GetAt = DateTime.UtcNow,
                Houses = houses
            });
        }

        // Delete Houses in DataBase
        [HttpDelete("UpDateHouses")]
        public async Task<IActionResult> Delete(int id)
        {
            var exist = await _unitofwork.Repository<Warehouse>().FindAsync(h => h.Id == id);
            if (exist == null)
            {
                return Conflict(new
                {
                    message = $" ❌ Not Found Houses Use Id: ({id}) In Database..!",
                    GetAt = DateTime.UtcNow,
                    Houses = exist
                });
            }
            var houses = await _services.DeleteWareHouse(id);
            if (houses == null)
                return NotFound("❌ Not Found Houses In Database..!");

            return Ok(new
            {
                message = $" ✅ Delete Houses Name: ({exist.Name}) (❁´◡`❁) successfully.",
                GetAt = DateTime.UtcNow,
                Houses = houses
            });
        }
    }
}
