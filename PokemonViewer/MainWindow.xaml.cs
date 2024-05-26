using PokemonViewer.Models;
using PokemonViewer.Services;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PokemonViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PokemonService _pokemonService;
        PokemonModel? _currentPokemon;
        PokemonImageType _currentImageType = PokemonImageType.Normal;

        public MainWindow()
        {
            InitializeComponent();
            _pokemonService = new PokemonService();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string pokemonName = pokemonNameTextBox.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(pokemonName)) return;

            try
            {
                var pokemon = await _pokemonService.GetPokemonDetails(pokemonName);
                if (pokemon == null)
                    return;
                _currentPokemon = pokemon;
                _currentImageType = PokemonImageType.Normal;
                UpdateView();
            }
            catch (Exception)
            {
                ShowError();
            }
        }

        private void UpdateView()
        {
            if (_currentPokemon == null)
                return;
            pokemonImage.Source = _currentImageType == PokemonImageType.Normal ? new BitmapImage(new Uri(_currentPokemon.ImageUrl!)) : new BitmapImage(new Uri(_currentPokemon.ShinyImageUrl!));
            pokemonAbilities.Text = "Abilities: " + string.Join(", ", _currentPokemon.Abilities!);
            pokemonTypes.Text = "Types: " + string.Join(", ", _currentPokemon.Types!);
            pokemonRarity.Text = "Rarity: " + (_currentImageType == PokemonImageType.Normal ? "Normal" : "Shiny");
        }

        private void ShowError()
        {
            pokemonImage.Source = GetErrorImage();
            pokemonAbilities.Text = "Abilities:";
            pokemonTypes.Text = "Types:";
            pokemonRarity.Text = "Rarity:";
        }

        private BitmapImage GetErrorImage()
        {
            var image = new BitmapImage();
            using (var stream = File.OpenRead("Assets/pokemon_not_found.png"))
            {
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        private void PokemonImage_Click(object sender, RoutedEventArgs e)
        {
            _currentImageType = _currentImageType == PokemonImageType.Normal ? PokemonImageType.Shiny : PokemonImageType.Normal;
            UpdateView();
        }

        private void PokemonNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (pokemonNameTextBox.Text == "Enter Pokémon Name...")
            {
                pokemonNameTextBox.Text = "";
                pokemonNameTextBox.Foreground = SystemColors.ControlTextBrush;
            }
        }

        private void PokemonNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(pokemonNameTextBox.Text))
            {
                pokemonNameTextBox.Text = "Enter Pokémon Name...";
                pokemonNameTextBox.Foreground = Brushes.Gray;
            }
        }

        enum PokemonImageType
        {
            Normal,
            Shiny
        }
    }
}