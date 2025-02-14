using MedInvEquip.Data;
using MedInvEquip.Models;
using MedInvEquip.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedInvEquip.Controllers
{
    public class MedicalEquipmentController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        private readonly AppDbContext dbContext;

        public MedicalEquipmentController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public AppDbContext DbContext { get; }


        //This method is just for viewing the form
        [HttpGet]
        //This means we are only allowing the logged-in users for adding data. This is just form.
        [Authorize]
        public IActionResult Add()
        {
            return View();
        }


        //This method captures form data, pushes it to the appropriate database table.
        [HttpPost]
        //This means we are only allowing the logged-in users for pushing data on table.
        [Authorize]
        public async  Task<IActionResult> Add(AddEquipmentViewModel viewModel)
        {
            MedicalEquipment medicalData = new MedicalEquipment()
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                Quantity = viewModel.Quantity,
                Price = viewModel.Price,
            };
            await dbContext.MedicalEquipments.AddAsync(medicalData);

            await dbContext.SaveChangesAsync();

            return RedirectToAction("List", "MedicalEquipment");
        }

        //Without pagination and sorting
        //[HttpGet]
        //public async Task<IActionResult> List()
        //{
        //    var data = await dbContext.MedicalEquipments.ToListAsync();

        //    return View(data);
        //}

        //With pagination and sorting
        //[HttpGet]
        //public async Task<IActionResult> List(int page = 1, string sortOrder = "asc")
        //{
        //    int pageSize = 10;  // Number of items per page

        //    // Calculate total number of records
        //    var totalRecords = await dbContext.MedicalEquipments.CountAsync();

        //    // Sorting logic based on price
        //    IQueryable<MedicalEquipment> medicalEquipments = dbContext.MedicalEquipments;

        //    // Apply sorting based on the sortOrder parameter
        //    if (sortOrder == "asc")
        //    {
        //        medicalEquipments = medicalEquipments.OrderBy(m => m.Price);
        //    }
        //    else
        //    {
        //        medicalEquipments = medicalEquipments.OrderByDescending(m => m.Price);
        //    }

        //    // Pagination logic: skip records based on the page number
        //    var equipmentPagedList = await medicalEquipments
        //        .Skip((page - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToListAsync();

        //    // ViewData to pass additional info to the view
        //    ViewData["SortOrder"] = sortOrder;
        //    ViewData["CurrentPage"] = page;
        //    ViewData["TotalPages"] = (int)Math.Ceiling(totalRecords / (double)pageSize);

        //    return View(equipmentPagedList);
        //}


        //With searching, sorting and pagination
        [HttpGet]
        public async Task<IActionResult> List(int page = 1, string sortOrder = "asc", string searchQuery = "")
        {
            int pageSize = 10;  // This is number of items per page

            // This calculate the total number of records or rows in MedicalEquipment table
            var totalRecordsQuery = dbContext.MedicalEquipments.AsQueryable();

            // This applies search filter
            if (!string.IsNullOrEmpty(searchQuery))
            {
                totalRecordsQuery = totalRecordsQuery.Where(e => e.Name.Contains(searchQuery));
            }

            var totalRecords = await totalRecordsQuery.CountAsync();

            // This is sorting logic based on price
            IQueryable<MedicalEquipment> medicalEquipments = totalRecordsQuery;

            if (sortOrder == "asc")
            {
                medicalEquipments = medicalEquipments.OrderBy(m => m.Price);
            }
            else
            {
                medicalEquipments = medicalEquipments.OrderByDescending(m => m.Price);
            }

            // This is pagination logic: skip records based on the page number
            var equipmentPagedList = await medicalEquipments
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // This can ViewData to pass additional info to the view
            ViewData["SortOrder"] = sortOrder;
            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = (int)Math.Ceiling(totalRecords / (double)pageSize);
            ViewData["SearchQuery"] = searchQuery;

            return View(equipmentPagedList);
        }


        //This method is for viewing form for editing data
        [HttpGet]
        //This means we are only allowing the logged-in users for editing data. This is just for for edit.
        [Authorize]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var medicalEquip = await dbContext.MedicalEquipments.FindAsync(Id);

            return View(medicalEquip);
        }


        //This method is to push the changes in editied data
        [HttpPost]
        //This means we are only allowing the logged-in users for pushing edited data on db.
        [Authorize]
        public async Task<IActionResult> Edit(MedicalEquipment editedMedEqip)
        {
            var editedInfo = await dbContext.MedicalEquipments.FindAsync(editedMedEqip.Id);

            if (editedInfo != null)
            {
                editedInfo.Name = editedMedEqip.Name;
                editedInfo.Description = editedMedEqip.Description;
                editedInfo.Quantity = editedMedEqip.Quantity;
                editedInfo.Price = editedMedEqip.Price;

                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "MedicalEquipment");
        }


        //This method deletes the data from the database table
        [HttpPost]
        //This means we are only allowing the logged-in users for deleting data from table
        [Authorize]
        public async Task<IActionResult> Delete(MedicalEquipment delEquip)
        {
            var deleteRow = await dbContext.MedicalEquipments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == delEquip.Id);
            if (deleteRow is not null)
            {
                dbContext.MedicalEquipments.Remove(delEquip); 
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "MedicalEquipment");
        }
    }
}
