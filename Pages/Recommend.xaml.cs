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
        private ImdbContext _context = new ImdbContext();
        public Recommend()
        {
            InitializeComponent();
            LoadRecommendaAsync(); // Load the recommendations asynchronously
        }

        private async Task LoadRecommendaAsync()
        {
            var episodes = await _context.Episodes
                .Include(e => e.Title) // Include related Title information.
                .ThenInclude(t => t.Rating) // Include related Rating information.
                .OrderByDescending(e => e.Title.Rating.AverageRating) // Order episodes by average rating.
                .Take(10) // Take only the top 10 episodes.
                .Select(e => new // Project the data into a new anonymous object for binding.
                {
                    e.Title.PrimaryTitle, // The primary title of the episode.
                    SeasonNumber = e.SeasonNumber.HasValue ? "Season " + e.SeasonNumber.Value.ToString() : "Unknown Season",
                    EpisodeNumber = e.EpisodeNumber.HasValue ? "Episode " + e.EpisodeNumber.Value.ToString() : "Unknown Episode",
                    Rating = e.Title.Rating.AverageRating // The average rating of the episode.
                })
                .ToListAsync(); // Execute the query and convert the result to a list.

            // Set the EpisodesRecommendations ListView's ItemsSource property to the episodes list.
            EpisodesRecommendations.ItemsSource = episodes;
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = textSearch.Text.ToLower(); // Convert search text to lowercase for case-insensitive search

            var episodes = await _context.Episodes
                .Include(e => e.Title)
                .ThenInclude(t => t.Rating)
                .Where(e => e.Title.PrimaryTitle.ToLower().Contains(searchText))
                .OrderByDescending(e => e.Title.Rating.AverageRating)
                .Take(10)
                .Select(e => new
                {
                    e.Title.PrimaryTitle,
                                          
                    SeasonNumber = e.SeasonNumber.HasValue ? "Season " + e.SeasonNumber.Value.ToString() : "Unknown Season",
                    EpisodeNumber = e.EpisodeNumber.HasValue ? "Episode " + e.EpisodeNumber.Value.ToString() : "Unknown Episode",
                    Rating = e.Title.Rating.AverageRating // The average rating of the episode.
                })
                .ToListAsync(); // Execute the query and convert the result to a list.

            // Set the EpisodesRecommendations ListView's ItemsSource property to the filtered episodes list.
            EpisodesRecommendations.ItemsSource = episodes;
        }


    }
}
