using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WatchList.Models;
using WatchList.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

/*
 * Note:
 * Session States should be used instead of static variables
 * for better encapsulation and reduces the risk of concurrency issues
 * I HAVE SET UP A CLASS FOR THIS BUT NOT IMPLEMENTED. 
 * IT IS IN INFRASTRUCTURE FOLDER, FILE CALLED SESSIONEXTENSIONS.CS
 */


namespace WatchList.Controllers
{

    public class AccountController : Controller
    {

        private readonly IInfoRepository _infoRepository;
        private readonly UserServices _userServices;
        private static LoginModel? _loginModel;
        private static FinalViewModel? _finalViewModel;
        public const int pageSize = 10;

        public AccountController(UserServices userServices,IInfoRepository infoRepository)
        {
            _userServices = userServices;
            _infoRepository = infoRepository;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
                return View(loginModel);

            if (!ValidateCredentials(ref loginModel))
            {
                ModelState.AddModelError("","Invalid Username or Password");
                return View(loginModel);
            }
           
            _loginModel = loginModel;

         return RedirectToAction("WatchList");

        }

        public IActionResult Logout()
        {
            _loginModel = null;
            return RedirectToAction("Login");

        }

        public IActionResult WatchList(int listPage = 1)
        {
            int userID = _loginModel?.UserID ?? 0;
            int totalItems = _infoRepository.GetTotalItemCount(userID);

            var pagingInfo = new PagingInfo
            {
                CurrentPage = listPage,
                ItemsPerPage = pageSize,
                TotalItems = totalItems
            };

            var series = _infoRepository.SeriesInfos
                .Where(s=>s.UserID == userID)
                .OrderByDescending(s => s.SeriesInfoID)
                .Skip((listPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            

            _finalViewModel = new FinalViewModel
            {
                LoginModel = _loginModel ?? new LoginModel(),
                InfoRepository = _infoRepository ,
                PagingInfo = pagingInfo

            };

            return View("Display",_finalViewModel);
        }
 
        [HttpGet]
        public IActionResult FilterByGenre(string? genre, int listPage = 1)
        {
            int userID = _loginModel?.UserID ?? 0;
            

            string? selectedGenre = genre;

            _finalViewModel = new FinalViewModel();

            if(genre == null)
            {
                _finalViewModel.SelectedGenre = "All";
            }
            else
            {
                _finalViewModel.SelectedGenre = genre;
            }

            if (genre == "All" || genre == null)
            {
                selectedGenre = null;
            }

            // Filter the series by genre if a genre is selected
            var filteredSeries = string.IsNullOrEmpty(selectedGenre)
                ? _infoRepository.SeriesInfos.Where(s => s.UserID == userID)
                : _infoRepository.SeriesInfos.Where(s => s.UserID == userID && s.Genre == genre);

            int totalItems = filteredSeries.Count();

            // Paginate the filtered series
            var series = filteredSeries
                .OrderBy(s => s.TitleWatched)
                .Skip((listPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var pagingInfo = new PagingInfo
            {
                CurrentPage = listPage,
                ItemsPerPage = pageSize,
                TotalItems = totalItems
            };

            _finalViewModel = new FinalViewModel
            {
                LoginModel = _loginModel ?? new LoginModel(),
                InfoRepository = _infoRepository, //need all available genre in user's repo
                PagingInfo = pagingInfo,
                FilteredSeries = series,
                SelectedGenre = _finalViewModel.SelectedGenre
            };
            return View("FullList",_finalViewModel);
        }

        public IActionResult Edit(int infoID)
        {
            var seriesInfo = _infoRepository.GetSeriesInfoByID(infoID, includeUser: true);
            return View(seriesInfo);
        }


        [HttpPost]
        public IActionResult Edit(SeriesInfo viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.UserID = _loginModel?.UserID ?? 0;
                var existingItem = _infoRepository.GetSeriesInfoByAttributes(viewModel);

                if (existingItem != null)
                {
                    ModelState.AddModelError("", "This item already exists in your list.");
                    return View(viewModel);
                }
                _infoRepository.SaveInfo(viewModel);

                return RedirectToAction("WatchList");
               
            }

            // If the model state is not valid
            return View(viewModel);

        }

        public IActionResult Add() => View();

        [HttpPost]
        public IActionResult Add(SeriesInfo seriesInfo)
        {

            if (_loginModel != null && _loginModel.UserID.HasValue)
            {
                seriesInfo.UserID = (int)_loginModel.UserID;
                var existingItem = _infoRepository.GetSeriesInfoByAttributes(seriesInfo);

                if(existingItem != null)
                {
                    ModelState.AddModelError("", "This item already exists in your list.");
                    return View(seriesInfo);
                }

                if (ModelState.IsValid)
                {
                    _infoRepository.CreateInfo(seriesInfo);
                    return RedirectToAction("WatchList");
                }
            }
            return View(seriesInfo);
        }

        public ViewResult AboutSite() { 
           _finalViewModel = new FinalViewModel
           {
               LoginModel = _loginModel ?? new LoginModel()
           };

            return View(_finalViewModel); 
        }

        public IActionResult Delete(int infoID)
        {
            var seriesInfo = _infoRepository.GetSeriesInfoByID(infoID, includeUser: true);
            return View(seriesInfo);
            
        }

        [HttpPost]
        public IActionResult Delete(SeriesInfo seriesInfo)
        {
            if(_loginModel != null && _loginModel.UserID.HasValue)
            {
                seriesInfo.UserID= (int)_loginModel.UserID;

                    _infoRepository.DeleteInfo(seriesInfo);
                
                return RedirectToAction("WatchList");

            }
            return View(seriesInfo);
        }

        private bool ValidateCredentials(ref LoginModel loginModel)
        {

            if(loginModel.LoginName == null || loginModel.Password == null)
                return false;

            int? userId = _userServices.GetID(loginModel.LoginName,loginModel.Password);
            
            if(!userId.HasValue)
                return false;

            if(_loginModel == null)
                _loginModel = new LoginModel();
            

            loginModel.UserID = userId;
            return true;

        }
    }
}
