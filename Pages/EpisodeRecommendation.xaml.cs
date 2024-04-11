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
    /// </summary>
    public partial class EpisodeRecommendation : Page
    {
        private ImdbContext _context = new ImdbContext();

        public EpisodeRecommendation()
        {
            InitializeComponent();
        }

        private async void OnSearch(object sender, RoutedEventArgs e)
        {
            // Get the search keyword from the text box and convert it to lower case for case-insensitive search
            var searchKeyword = textSearch.Text.ToLower();

            // Query the database asynchronously to get episodes that contain the search keyword in their titles,
            // include associated rating information, order the results by number of votes in descending order,
            // and select only the necessary fields
            var episodes = await _context.Episodes
                .Include(e => e.Title)               // Include Title details
                .ThenInclude(t => t.Rating)          // Include Rating details within Title
                .Where(e => e.Title.PrimaryTitle.ToLower().Contains(searchKeyword))  // Filter by search keyword
                .OrderByDescending(e => e.Title.Rating.NumVotes)  // Order by number of votes
                .Select(e => new                     // Project the results into a new anonymous object
                {
                    Title = e.Title.PrimaryTitle,    // Episode title
                    NumVotes = e.Title.Rating.NumVotes  // Number of votes for the episode
                })
                .ToListAsync();                       // Execute the query and get the results as a list

            // Set the ItemsSource of the ListView to the list of episodes retrieved
            ListViewEpisodes.ItemsSource = episodes;
        }
    }
}
