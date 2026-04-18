using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Star_Security_Service.Data;
using Star_Security_Service.Models;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;


namespace Star_Security_Service.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly Star_security_ServiceContext db;
        private readonly IHttpContextAccessor contx;

        public UserController(ILogger<UserController> logger, Star_security_ServiceContext db, IHttpContextAccessor contx)
        {
            _logger = logger;
            this.db = db;
            this.contx = contx;
        }

        // Index Page
        public IActionResult Index()
        {
            var model = new IndexViewModel
            {
                Net = db.Networks.ToList(),
                Tes = db.Testimonials.ToList(),
                Man = db.MannedGuardings.ToList(),
            };
            return View(model);
        }

        // About Page
        public IActionResult About()
        {
            return View();
        }

        // Contact Page
        public IActionResult Contact()
        {
            if (HttpContext.Session.GetString("useremail") == null)
            {
                return RedirectToAction("Login", "User");
            }
            return View();
        }

        [HttpPost]
        public IActionResult ContactAddData(Contact newData)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(newData);
                db.SaveChanges();
                TempData["SuccessMessageContact"] = "Form has been filled successfully!";
                return RedirectToAction("Contact");
            }

            return View("Contact", newData);
        }


        public IActionResult Signup()
        {
            if (HttpContext.Session.GetString("useremail") != null)
            {
                return RedirectToAction("Index", "User");
            }

            return View(new RegisterationUser());
        }

        [HttpPost]
        public IActionResult Signup(RegisterationUser newuser)
        {
            if (HttpContext.Session.GetString("useremail") != null)
            {
                return RedirectToAction("Index", "User");
            }

            var existingUser = db.RegisterationUsers.FirstOrDefault(u => u.Email == newuser.Email);
            if (existingUser != null)
            {
                TempData["ErrorMessageUserSignup"] = "Email is already registered!";
                return View(newuser);
            }

            if (ModelState.IsValid)
            {
                db.RegisterationUsers.Add(newuser);
                db.SaveChanges();

                TempData["SuccessMessageSignup"] = "Registration successful! You can now log in.";
                return RedirectToAction("Login", "User");
            }

            return View(newuser);
        }


        // Login Page

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("useremail") != null)
            {
                return RedirectToAction("Index", "User");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(RegisterationUser userAuth)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessageLogin"] = "Invalid input! Please check your credentials.";
                return View();
            }

            var front_useremail = userAuth.Email;
            var front_userpass = userAuth.Password;

            var fetchuser = db.RegisterationUsers.Where(user => user.Email == front_useremail).FirstOrDefault();

            if (fetchuser != null && fetchuser.Password == front_userpass)
            {
                // Create session
                contx.HttpContext.Session.SetString("useremail", fetchuser.Email);
                contx.HttpContext.Session.SetString("userpass", fetchuser.Password);
                contx.HttpContext.Session.SetString("username", fetchuser.Name);

                TempData["SuccessMessageLogin"] = "Login successful!";
                return RedirectToAction("Index", "User");
            }

            TempData["ErrorMessageLogin"] = "Invalid input! Please check your credentials.";
            return View();


        }


        //Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }



        // Business Page (Manned Guarding)
        public IActionResult MannedGuarding(MannedGuardingModelView man)
        {
            if (HttpContext.Session.GetString("useremail") == null)
            {
                return RedirectToAction("Login", "User");
            }
            man.MannedGuardingList = db.MannedGuardings.ToList();
            return View(man);
        }



        // Business Page (Our Network)
        public IActionResult OurNetwork()
        {

            var userdata = db.Networks.ToList();
            return View(userdata);
        }


        // GET: BookNow Form
        [HttpGet]
        public IActionResult Booking()
        {
            if (HttpContext.Session.GetString("useremail") == null)
            {
                return RedirectToAction("Login", "User");
            }
            BookingViewModel viewModel = new BookingViewModel
            {
                Name = HttpContext.Session.GetString("username"),
                Email = HttpContext.Session.GetString("useremail"),
                Services = db.MannedGuardings.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Booking(BookingViewModel model)
        {
            if (HttpContext.Session.GetString("useremail") == null)
            {
                return RedirectToAction("Login", "User");
            }

            //if (!ModelState.IsValid)
            //{
            //    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            //    TempData["ErrorMessageBooking"] = "Validation failed: " + string.Join(", ", errors);

            //    model.Emp = db.EmployeeInformations.Where(e => e.Action == "Accepted").ToList();
            //    model.Services = db.MannedGuardings.ToList();
            //    return View(model);
            //}

            Booking newBooking = new Booking
            {
                Name = model.Name,
                Email = model.Email,
                EmployeeId = model.EmployeeId,
                ServiceId = model.ServiceId,
                BookingDatetime = model.BookingDatetime
            };

            db.Bookings.Add(newBooking);
            db.SaveChanges();
            TempData["SuccessMessageBooking"] = "Your booking has been placed successfully!";
            return RedirectToAction("Profile","User");
        }


        [HttpGet]
        public JsonResult GetEmployeesByService(int serviceId)
        {
            var employees = db.EmployeeInformations
                .Where(e => e.ServiceId == serviceId && e.Action == "Accepted")
                .Select(e => new
                {
                    Id = e.Id,
                    Name = e.Name,
                    Grade = e.Grade
                })
                .ToList();

            return Json(employees);
        }


        [HttpPost]
        public IActionResult DeleteBooking(int id)
        {
            if (HttpContext.Session.GetString("useremail") == null)
            {
                return RedirectToAction("Login", "User");
            }
            var booking = db.Bookings.FirstOrDefault(b => b.Id == id && b.Email == HttpContext.Session.GetString("useremail"));
            if (booking != null)
            {
                db.Bookings.Remove(booking);
                db.SaveChanges();
                TempData["SuccessMessageDeleteBooking"] = "Booking deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Booking not found!";
            }

            return RedirectToAction("Profile");
        }



        //Profile
        private bool frozen()
        {
            var userEmail = HttpContext.Session.GetString("useremail");

            if (!string.IsNullOrEmpty(userEmail))
            {
                var employeeRecord = db.EmployeeInformations.FirstOrDefault(e => e.Email == userEmail);
                if (employeeRecord != null && employeeRecord.Action == "Freeze")
                {
                    return true;
                }
            }

            return false;
        }


        public IActionResult Profile()
        {
            if (HttpContext.Session.GetString("useremail") == null)
            {
                return RedirectToAction("Index", "User");
            }

            if (frozen())
            {
                TempData["ErrorMessagefrozen"] = "Your profile is frozen by Admin.";
                return RedirectToAction("Index", "User");
            }

            string loggedInUserEmail = HttpContext.Session.GetString("useremail");
            string loggedInUserName = HttpContext.Session.GetString("username");

            // Check Employee Action
            var employeeRecord = db.EmployeeInformations.FirstOrDefault(e => e.Email == loggedInUserEmail);
            bool isAccepted = employeeRecord != null && employeeRecord.Action == "Accepted";

            var model = new ProfileViewModel
            {
                Bookings = isAccepted
                    ? db.Bookings
                        .Where(b => db.EmployeeInformations.Any(e => e.Id == b.EmployeeId && e.Name == loggedInUserName))
                        .Select(b => new Booking
                        {
                            Id = b.Id,
                            Name = b.Name,
                            Email = b.Email,
                            Employee = db.EmployeeInformations.FirstOrDefault(e => e.Id == b.EmployeeId),
                            Service = db.MannedGuardings.FirstOrDefault(s => s.Id == b.ServiceId),
                            BookingDatetime = b.BookingDatetime
                        })
                        .ToList()
                    : db.Bookings
                        .Where(b => b.Email == loggedInUserEmail)
                        .Select(b => new Booking
                        {
                            Id = b.Id,
                            Name = b.Name,
                            Email = b.Email,
                            Employee = db.EmployeeInformations.FirstOrDefault(e => e.Id == b.EmployeeId),
                            Service = db.MannedGuardings.FirstOrDefault(s => s.Id == b.ServiceId),
                            BookingDatetime = b.BookingDatetime
                        })
                        .ToList(),

                EmployeeInfo = db.EmployeeInformations
                    .Where(e => e.Email == loggedInUserEmail && e.Action == "Accepted")
                    .ToList()
            };

            return View(model);
        }




        //Career With Us
        public IActionResult CareerWithUs()
        {
            if (HttpContext.Session.GetString("useremail") == null)
            {
                return RedirectToAction("Login", "User");
            }

            EmployeeInformationViewModel viewModel = new EmployeeInformationViewModel
            {
                Name = HttpContext.Session.GetString("username"),
                Email = HttpContext.Session.GetString("useremail"),
                Services = db.MannedGuardings.ToList(),
                employeeInformation = new EmployeeInformation()
            };

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult CareerWithUs(EmployeeInformation model)
        {
            if (ModelState.IsValid)
            {
                db.EmployeeInformations.Add(model);
                db.SaveChanges();
                TempData["SuccessMessageCareerWithUs"] = "Your Request has been sent wait for admin's responce!";
                return RedirectToAction("Profile");
            }

            //model.Services = db.MannedGuardings.ToList();

            return View(model);
        }



        //Review
        [HttpGet]
        public IActionResult CreateReview()
        {
            if (HttpContext.Session.GetString("useremail") == null)
            {
                return RedirectToAction("Login", "User");
            }

            var model = new Testimonial
            {
                Name = HttpContext.Session.GetString("username")
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult CreateReview(Testimonial newTestimonial)
        {
            if (HttpContext.Session.GetString("useremail") == null)
            {
                return RedirectToAction("Login", "User");
            }

            if (ModelState.IsValid)
            {
                newTestimonial.Name = HttpContext.Session.GetString("username");
                db.Testimonials.Add(newTestimonial);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Your review has been submitted!";
                return RedirectToAction("Index","User");
            }

            return View(newTestimonial);
        }
    }
}