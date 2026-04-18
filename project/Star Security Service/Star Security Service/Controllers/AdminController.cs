using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Star_Security_Service.Data;
using Star_Security_Service.Models;
using System;
using System.IO;
using System.Linq;

namespace Star_Security_Service.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly Star_security_ServiceContext db;
        private readonly IHttpContextAccessor contx;
        private readonly IWebHostEnvironment env;

        public AdminController(ILogger<AdminController> logger, Star_security_ServiceContext db, IHttpContextAccessor contx, IWebHostEnvironment env)
        {
            _logger = logger;
            this.db = db;
            this.contx = contx;
            this.env = env;
        }

        //  Index Page
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("useremailadmin") == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            var model = new DashViewModel
            {
                Con = db.Contacts.ToList(),
                                Tes = db.Testimonials.ToList()
            };

            return View(model);

        }

        public IActionResult UserLogInfo()
        {
            if (HttpContext.Session.GetString("useremailadmin") == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            var userdata = db.RegisterationUsers
                .Select(u => new RegisterationUser
                {
                    Id = u.Id,
                    Name = u.Name ?? "N/A",
                    Email = u.Email ?? "No Email",
                    Password = u.Password ?? "No Password"
                })
                .ToList();

            return View(userdata);
        }


        public IActionResult Booking()
        {
            if (HttpContext.Session.GetString("useremailadmin") == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            var userdata = db.Bookings
                .Include(b => b.Employee)  // Load Employee details
                .Include(b => b.Service)   // Load Service details
                .ToList();

            return View(userdata);
        }


        // Signup Page (GET)
        [HttpGet]
        public IActionResult Signup()
        {
            if (HttpContext.Session.GetString("useremailadmin") != null)
            {
                return RedirectToAction("Index", "Admin");
            }

            AdminRegisterationCustomViewModel viewModel = new AdminRegisterationCustomViewModel()
            {
                RoleList = db.AdminRegisterationRoles.ToList(),
                registrationFormData = new AdminRegisteration()
            };

            return View(viewModel);
        }

        //  Signup Page (POST)
        [HttpPost]
        public IActionResult Signup(AdminRegisterationCustomViewModel newuser)
        {
            var existingUser = db.AdminRegisterations.FirstOrDefault(u => u.Email == newuser.registrationFormData.Email);
            if (existingUser != null)
            {
                TempData["ErrorMessageSignup"] = "Email is already registered!";
                return View(newuser);
            }

            if (ModelState.IsValid)
            {
                db.AdminRegisterations.Add(newuser.registrationFormData);
                db.SaveChanges();

                TempData["SuccessMessageSignup"] = "Registration successful! You can now log in.";
                return RedirectToAction("Login", "Admin");
            }

            return View(newuser);
        }

        //  Login Page (GET)
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("useremailadmin") != null)
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }

        //  Login Page (POST)
        [HttpPost]
        public IActionResult Login(AdminRegisteration userAuth)
        {
            var user = db.AdminRegisterations.FirstOrDefault(u => u.Email == userAuth.Email);

            if (user != null && user.Password == userAuth.Password)
            {
                contx.HttpContext.Session.SetString("useremailadmin", user.Email);
                contx.HttpContext.Session.SetString("usernameadmin", user.Name);
                contx.HttpContext.Session.SetString("userroleadmin", user.Role.ToString());

                if (user.Role == 1 || user.Role == 2)
                {
                    return RedirectToAction("Index", "Admin");
                }
            }

            TempData["ErrorMessageLogin"] = "Invalid email or password!";
            return View();
        }

        // Logout Functionality
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Admin");
        }

        // Utility: Check if Admin is Logged In
        private bool IsAdminLoggedIn()
        {
            var role = HttpContext.Session.GetString("userroleadmin");
            return role == "2";
        }

        // Add Manned Guarding 
        [HttpGet]
        public IActionResult MannedGuarding()
        {
            if (!IsAdminLoggedIn())
            {
                TempData["ErrorMessageMannedGuarding"] = "Access Denied: Only Admins can perform this action.";
                return RedirectToAction("Index", "Admin");
            }

            var model = new MannedGuardingModelView
            {
                MannedGuardingList = db.MannedGuardings.ToList() ?? new List<MannedGuarding>(),
                                Tes = db.Testimonials.ToList()

            };

            return View(model);
        }


        // Add Manned Guarding (Upload Image)
        [HttpPost]
        public IActionResult MannedGuarding(MannedGuardingModelView man)
        {
            if (!IsAdminLoggedIn())
            {
                TempData["ErrorMessageMannedGuarding"] = "Access Denied: Only Admins can view this page.";
                return RedirectToAction("Index", "Admin");
            }
            string fileName = "";
            if (man.Image != null)
            {
                string folder = Path.Combine(env.WebRootPath, "img");
                fileName = Guid.NewGuid().ToString() + " " + man.Image.FileName;
                string filePath = Path.Combine(folder, fileName);
                man.Image.CopyTo(new FileStream(filePath, FileMode.Create));

                MannedGuarding manned = new MannedGuarding()
                {
                    Title = man.Title,
                    Description = man.Description,
                    ImagePath = fileName
                };


                db.MannedGuardings.Add(manned);
                db.SaveChanges();
                return RedirectToAction("MannedGuarding");
            }
            man.MannedGuardingList = db.MannedGuardings.ToList();
            return View(man);
        }


        [HttpGet]
        public IActionResult EditMannedGuarding(int id)
        {
            if (!IsAdminLoggedIn())
            {
                TempData["ErrorMessageMannedGuarding"] = "Access Denied: Only Admins can perform this action.";
                return RedirectToAction("Index", "Admin");
            }

            var mannedGuarding = db.MannedGuardings.FirstOrDefault(m => m.Id == id);
            if (mannedGuarding == null)
            {
                return NotFound();
            }

            var model = new MannedGuardingModelView
            {
                Id = mannedGuarding.Id,
                Title = mannedGuarding.Title,
                Description = mannedGuarding.Description,
                Items = mannedGuarding.Items, // Ensure the Item field is mapped
                MannedGuardingList = db.MannedGuardings.ToList()
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult EditMannedGuarding(MannedGuardingModelView man)
        {
            if (!IsAdminLoggedIn())
            {
                TempData["ErrorMessageMannedGuarding"] = "Access Denied: Only Admins can perform this action.";
                return RedirectToAction("Index", "Admin");
            }

            var existingMannedGuarding = db.MannedGuardings.FirstOrDefault(m => m.Id == man.Id);
            if (existingMannedGuarding == null)
            {
                return NotFound();
            }

            // Update only Title, Description, and Item (Ignore Image)
            existingMannedGuarding.Title = man.Title;
            existingMannedGuarding.Description = man.Description;
            existingMannedGuarding.Items = man.Items;

            db.MannedGuardings.Update(existingMannedGuarding);
            db.SaveChanges();

            return RedirectToAction("MannedGuarding");
        }


        //Manned Guarding Delete
        [HttpPost]
        public IActionResult DeleteMannedGuarding(int id)
        {
            if (HttpContext.Session.GetString("useremailadmin") == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            var mannedGuarding = db.MannedGuardings.FirstOrDefault(mg => mg.Id == id);
            if (mannedGuarding != null)
            {
                db.MannedGuardings.Remove(mannedGuarding);
                db.SaveChanges();
                TempData["SuccessMessageDeleteMannedGuarding"] = "Service deleted successfully!";
            }
            else
            {
                TempData["ErrorMessageDeleteMannedGuarding"] = "Service not found!";
            }

            return RedirectToAction("MannedGuarding", "Admin");
        }



        //career with us
        public IActionResult CareerWithUs()
        {
            if (HttpContext.Session.GetString("useremailadmin") == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var userdata = db.EmployeeInformations.ToList();
            return View(userdata);

        }



        [HttpPost]
        public IActionResult UpdateEmployeeAction(int id)
        {
            if (HttpContext.Session.GetString("useremailadmin") == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            var EmployeeInformation = db.EmployeeInformations.FirstOrDefault(mg => mg.Id == id);
            if (EmployeeInformation != null)
            {
                EmployeeInformation.Action = "Accepted";
                db.SaveChanges();
            }
            else
            {
                TempData["ErrorMessageUpdateEmployeeInformation"] = "Service not found!";
            }

            return RedirectToAction("CareerWithUs", "Admin");
        }


        [HttpPost]
        public IActionResult DeleteEmployeeRequest(int id)
        {
            if (HttpContext.Session.GetString("useremail") == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            var employeeRequest = db.EmployeeInformations.FirstOrDefault(e => e.Id == id);
            if (employeeRequest != null)
            {
                db.EmployeeInformations.Remove(employeeRequest);
                db.SaveChanges();
                TempData["SuccessMessageDeleteEmployee"] = "Employee request deleted successfully!";
            }
            else
            {
                TempData["ErrorMessageDeleteEmployee"] = "Employee request not found!";
            }

            return RedirectToAction("CareerWithUs", "Admin");
        }


        [HttpGet]
        public IActionResult EditEmployeeRequest(int id)
        {
            if (HttpContext.Session.GetString("useremailadmin") == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            var employee = db.EmployeeInformations.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                TempData["ErrorMessageEditEmployeeRequest"] = "Employee record not found!";
                return RedirectToAction("CareerWithUs", "Admin");
            }

            return View(employee);
        }



        [HttpPost]
        public IActionResult EditEmployeeRequest(EmployeeInformation updatedEmployee)
        {
            if (HttpContext.Session.GetString("useremailadmin") == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            var employee = db.EmployeeInformations.FirstOrDefault(e => e.Id == updatedEmployee.Id);
            if (employee == null)
            {
                TempData["ErrorMessageEditEmployeeRequest"] = "Employee record not found!";
                return RedirectToAction("CareerWithUs", "Admin");
            }

            employee.Phonenumber = updatedEmployee.Phonenumber ?? employee.Phonenumber;
            employee.Qualification = updatedEmployee.Qualification ?? employee.Qualification;
            employee.Grade = updatedEmployee.Grade ?? employee.Grade;
            employee.Client = updatedEmployee.Client ?? employee.Client;
            employee.Achievements = updatedEmployee.Achievements ?? employee.Achievements;
            employee.Action = updatedEmployee.Action ?? employee.Action;

            db.SaveChanges();
            TempData["SuccessMessage"] = "Employee record updated successfully!";
            return RedirectToAction("CareerWithUs", "Admin");
        }


        // Network Page

        public IActionResult Network()
        {
            if (HttpContext.Session.GetString("useremailadmin") == null)
            {
                return RedirectToAction("Login", "User");
            }
            var userdata = db.Networks.ToList();
            return View(userdata);

        }

        [HttpGet]
        public IActionResult CreateNetwork()
        {
            if (HttpContext.Session.GetString("useremailadmin") == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }

        [HttpPost]
        public IActionResult CreateNetwork(Network newData)
        {
            //if (ModelState.IsValid)
            //{
                db.Networks.Add(newData);
                db.SaveChanges();
                TempData["SuccessMessageNetwork"] = "Network entry added successfully!";
                return RedirectToAction("Network", "Admin");
            //}

            //return View(newData);
        }

        [HttpPost]
        public IActionResult DeleteNetwork(int id)
        {
            if (HttpContext.Session.GetString("useremailadmin") == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            var network = db.Networks.FirstOrDefault(n => n.Id == id);
            if (network != null)
            {
                db.Networks.Remove(network);
                db.SaveChanges();
                TempData["SuccessMessageNetwork"] = "Network entry deleted successfully!";
            }
            else
            {
                TempData["ErrorMessageNetwork"] = "Network entry not found!";
            }

            return RedirectToAction("Network", "Admin");
        }



    }
}
