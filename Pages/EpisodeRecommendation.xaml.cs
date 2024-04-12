using IMDB.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MovieRecommend.Pages
{
    /// <summary>
    /// Interaction logic for EpisodeRecommendation.xaml
    /// This page shows the top movies that received the most attention, ordered by the number of votes.
    /// Additional information like genre, start year, and runtime is also displayed.
    /// </summary>
    public partial class EpisodeRecommendation : Page
    {
        private ImdbContext _context = new ImdbContext();

        public EpisodeRecommendation()
        {
            InitializeComponent();
            LoadTopMoviesAsync(); // Automatically load the top movies when the page is loaded.
        }

        // Loads the top movies asynchronously
        private async Task LoadTopMoviesAsync()
        {
            var topMovies = await _context.Titles
                .Include(t => t.Rating)
                .Include(t => t.Genres)
                .OrderByDescending(t => t.Rating.NumVotes)
                .Take(10)
                .Select(t => new
                {
                    Title = t.PrimaryTitle,
                    Genres = string.Join(", ", t.Genres.Select(g => g.Name)),
                    StartYear = t.StartYear,
                    RuntimeMinutes = t.RuntimeMinutes,
                    NumVotes = t.Rating.NumVotes
                })
                .ToListAsync();

            ListViewTopMovies.ItemsSource = topMovies;
        }
        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = textSearch.Text.ToLower(); // Convert search text to lowercase for case-insensitive search

            var topMovies = await _context.Titles
                .Include(t => t.Rating)
                .Include(t => t.Genres)
                .Where(t => t.PrimaryTitle.ToLower().Contains(searchText)) // Filter by movies whose title contains the search text
                .OrderByDescending(t => t.Rating.NumVotes)
                .Take(10)
                .Select(t => new
                {
                    Title = t.PrimaryTitle,
                    Genres = string.Join(", ", t.Genres.Select(g => g.Name)),
                    StartYear = t.StartYear,
                    RuntimeMinutes = t.RuntimeMinutes,
                    NumVotes = t.Rating.NumVotes
                })
                .ToListAsync();

            ListViewTopMovies.ItemsSource = topMovies;
        }

    }
}
