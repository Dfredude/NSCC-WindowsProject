using System.Linq;
using System.Windows.Controls;
using System.Threading.Tasks;
using IMDB.Data;
using Microsoft.EntityFrameworkCore;

namespace MovieRecommend.Pages
{
    /// <summary>
    /// Interaction logic for DirectorRecommendation.xaml
    /// </summary>
    public partial class DirectorRecommendation : Page
    {
        private readonly ImdbContext _context = new ImdbContext();

        public DirectorRecommendation()
        {
            InitializeComponent();
            Loaded += DirectorRecommendation_Loaded;
        }

        // Event handler for when the page is loaded
        private async void DirectorRecommendation_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            await LoadRecentMoviesAsync(); // Load recent movies when the page is loaded
        }

        // Loads the most recent movies asynchronously
        private async Task LoadRecentMoviesAsync()
        {
            var recentMovies = await _context.Titles
                .OrderByDescending(t => t.StartYear)
                .Select(t => new
                {
                    Title = t.OriginalTitle,
                    t.StartYear
                })
                .Take(100)
                .ToListAsync();

            RecentMoviesListView.ItemsSource = recentMovies; // Set the data source for the list view
        }
    }
}
