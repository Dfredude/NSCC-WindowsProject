using IMDB.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MovieRecommend.Pages
{
    /// <summary>
    /// Interaction logic for Recommend.xaml
    /// </summary>
    public partial class Recommend : Page
    {
        ImdbContext imdbContext = new ImdbContext();
        public Recommend()
        {
            InitializeComponent();
            LoadRecommendaAsync(); // Load the recommendations asynchronously
        }

        public async Task LoadRecommendaAsync()
        {
            if (!imdbContext.Episodes.Local.Any()) imdbContext.Episodes.Load();
            // Select top 10 episodes
            var episodes = await imdbContext.Episodes.Include(e => e.Title).Take(10).ToListAsync();

            // Bind the episodes to the listbox
            EpisodesRecommendations.ItemsSource = episodes;
        }
    }
}
