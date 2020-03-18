﻿using Prism.Navigation;
using Prism.Services;
using PrismSportApp.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PrismSportApp.ViewModels
{
    public class ListLeaguesViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<League> Leagues { get; set; } = new ObservableCollection<League>();
        public League league { get; set; } = new League();
        Competitions League { get; set; } = new Competitions();
        public Links Links { get; set; } = new Links();


        public ICommand Tap { get; set; }

        INavigationService navigation;

        IPageDialogService dialogService;

        IApiServices apiServices;
        public ListLeaguesViewModel(IApiServices api, INavigationService navigationService, IPageDialogService pageDialog)
        {
            apiServices = api;
            navigation = navigationService;
            dialogService = pageDialog;
            GetLeagues();
           
            Tap = new Command(SelectLeague);
        }

        async void GetLeagues()
        {
            try
            {
                RestService.For<IApiServices>(Links.url);
                var response1 = await apiServices.GetLeagues();
                League = response1;
                //var show = League.competitions;
                var show = League.competitions.Where(elemento => elemento.Id == 2000 ||
                elemento.Id == 2001 ||
                elemento.Id == 2021 ||
                elemento.Id == 2015 ||
                elemento.Id == 2019 ||
                elemento.Id == 2017 ||
                elemento.Id == 2003 ||
                elemento.Id == 2002 ||
                elemento.Id == 2014);
                this.Leagues = show;
            }
            catch (Exception e)
            {
                await dialogService.DisplayAlertAsync("Advice", "Not connection to internet", "Ok");
            }
        }
        async void SelectLeague(object sender)
        {
            NavConstants navConstants = new NavConstants();
            league = (League)sender;
            var search = League.competitions.Where(elemento => elemento.Id == league.Id);
            if (search.Any())
            {
                var parameters = new NavigationParameters();
                parameters.Add("LeagueId", league.Id);
                parameters.Add("Name", league.Name);
                await navigation.NavigateAsync(new Uri(navConstants.TabMenu , UriKind.Relative), parameters);
            }
        }

        
    }
}