using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreProject.Domain.Identity;
using DotNetCoreProject.Web.ViewModels.Manage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DotNetCoreProject.Web.Controllers
{
    
    
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ManageController(UserManager<ApplicationUser> userManager
            , RoleManager<IdentityRole> roleManager
            , SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        //Kullanıcıları listeleme
      
        public IActionResult Index()
        {
            List<UserViewModel> users = _userManager.Users.Select(x =>
            new UserViewModel
            {
                Id = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName
            }).ToList();
            return View(users);
        }
        //rolleri listeleme

        public IActionResult Roles()
        {
            List<RoleViewModel> roles = _roleManager.Roles.Select(x =>
            new RoleViewModel
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();
            return View(roles);
        }
        // Rol oluşturma
        [Route("Roles/Create")]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        [Route("Roles/Create")]
        public async Task<IActionResult> CreateRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
               
                var newRole = await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = model.Name
                });
                if (newRole.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Hata Oluştu");
                }
            }
            return View(model);
        }
        //Rol güncelleme
        [Route("Roles/Edit/{id}")]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            RoleViewModel model = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(model);
        }
        [HttpPost]
        [Route("Roles/Edit/{id}")]
        public async Task<IActionResult> EditRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var updateRole = await _roleManager.FindByIdAsync(model.Id);
                updateRole.Name = model.Name;
                var update = await _roleManager.UpdateAsync(updateRole);
                if (update.Succeeded)
                {
                    return RedirectToAction("Roles", "Manage");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Güncellenirken bir hata oluştu");
            }
            return View(model);

        }
        [Route("Roles/Delete/{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            ViewBag.ErrorMessage = string.Empty;
            var usersInThisRole = await _userManager.GetUsersInRoleAsync(role.Name);
            if (usersInThisRole.Any())
            {

                string[] usersHas = usersInThisRole.Select(x => x.UserName).ToArray();
                var usersInThisRoleCommaSeperated = string.Join(", ", usersHas);
                ViewBag.ErrorMessage = $"Bu role sahip kullanicilar var: {usersInThisRoleCommaSeperated}";
            }
            var model = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(model);

        }
        [HttpPost]
        [Route("Roles/Delete/{id}")]
        public async Task<IActionResult> DeleteRole(string id, RoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);
           
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                ModelState.AddModelError(string.Empty, "Bir hata oluştu tekrar deneyin");           
            
            return View(model);

        }

        [HttpGet]
        [Route("Roles/Assign/{userId}")]
        public IActionResult AssignRole(string userId)
        {
            List<SelectListItem> roleList = _roleManager.Roles.Select(x => new SelectListItem
            {
                Value = x.Id,
                Selected = true,
                Text = x.Name
            }).ToList();
            AssignRoleViewModel roleModel = new AssignRoleViewModel
            {
                UserId = userId,
                RoleList = roleList
            };

            return View(roleModel);
        }

        [Route("Roles/Assign/{userId}")]
        public async Task<IActionResult> AssignRole(AssignRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                var role = await _roleManager.FindByIdAsync(model.RoleId);
                var assignRole = await _userManager.AddToRoleAsync(user, role.Name);
                if (assignRole.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Rol atanırken bir hata oluştu");
                }
            }
            return View(model);
        }
        [Route("Users/Detail/{userId}")]
        public async Task<IActionResult> UserDetail(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var model = new UserViewModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,

            };
            ViewBag.UserRoles = "Role sahip degil";
            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Any())
            {
                ViewBag.UserRoles = string.Join(", ", userRoles);
            }
            return View(model);
        }
        [Route("Roles/{roleId}")]
        public async Task<IActionResult> RoleDetail(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return RedirectToAction("Error", "Home");
            }
            RoleViewModel model = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            ViewBag.UsersInRole = "Bu role sahip herhangi bir kullanici bulunamadi!";
            if (usersInRole.Any())
            {
                string[] usersArr = usersInRole.Select(x => x.UserName).ToArray();
                string usersMsg = string.Join(", ", usersArr);
                ViewBag.UsersInRole = "Bu role sahip kullanicilar:  " + usersMsg;
            }

            return View(model);
        }

        //Kullanıcıyı rolden çıkar
        [Route("Roles/Revoke/{userId}")]
        public async Task<IActionResult> RevokeRole(string userId)
        {
            AssignRoleViewModel model = new AssignRoleViewModel();
            model.UserId = userId;
            var user = await _userManager.FindByIdAsync(userId);
            var userRolesList = await _userManager.GetRolesAsync(user);
            if (userRolesList.Any())
            {
                var userRoles = _roleManager.Roles.Where(x => userRolesList.Contains(x.Name)).ToList();
                model.RoleList = userRoles.Select(x => new SelectListItem
                {
                    Selected = false,
                    Text = x.Name,
                    Value = x.Id
                }).ToList();
            }
            else
            {
                model.RoleList = new List<SelectListItem>();
            }

            return View(model);
        }
        [HttpPost]
        [Route("Roles/Revoke/{userId}")]
        public async Task<IActionResult> RevokeRole(AssignRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.RoleId);
                var user = await _userManager.FindByIdAsync(model.UserId);
                var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                if (result.Succeeded)
                {
                    return RedirectToAction("UserDetail", new { userId = model.UserId });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Bir Hata oluştu");
                }
            }
            return View(model);
        }

    }
}