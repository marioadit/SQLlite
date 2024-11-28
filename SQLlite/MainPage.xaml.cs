using SQLlite.Models;

namespace SQLlite;

public partial class MainPage : ContentPage
{
    private Category _selectedCategory;

    public MainPage()
    {
        InitializeComponent();
        LoadCategoriesAsync();
    }

    private async void LoadCategoriesAsync()
    {
        try
        {
            var categories = await App.CategoryRepo.GetCategoriesAsync();
            CategoriesCollectionView.ItemsSource = categories;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load categories: {ex.Message}", "OK");
        }
    }

    private async void OnAddCategoryClicked(object sender, EventArgs e)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            await DisplayAlert("Validation", "Please enter a category name.", "OK");
            return;
        }

        var newCategory = new Category
        {
            Name = NameEntry.Text,
            Description = DescriptionEntry.Text
        };

        try
        {
            // Add new category to the repository
            await App.CategoryRepo.AddCategoryAsync(newCategory);

            // Clear input fields
            NameEntry.Text = string.Empty;
            DescriptionEntry.Text = string.Empty;

            // Reload categories
            LoadCategoriesAsync();

            // Show confirmation
            await DisplayAlert("Success", "Category added successfully!", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to add category: {ex.Message}", "OK");
        }
    }

    private async void OnUpdateSelectedCategoryClicked(object sender, EventArgs e)
    {
        if (_selectedCategory == null)
        {
            await DisplayAlert("No Selection", "Please select a category to update.", "OK");
            return;
        }

        // Show a custom popup to edit name and description
        var updatePage = new UpdateCategoryPage(_selectedCategory);
        updatePage.CategoryUpdated += async (s, updatedCategory) =>
        {
            try
            {
                await App.CategoryRepo.UpdateCategoryAsync(updatedCategory);
                await DisplayAlert("Success", "Category updated successfully.", "OK");
                LoadCategoriesAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to update category: {ex.Message}", "OK");
            }
        };

        await Navigation.PushModalAsync(updatePage);
    }

    private async void OnDeleteSelectedCategoryClicked(object sender, EventArgs e)
    {
        if (_selectedCategory == null)
        {
            await DisplayAlert("No Selection", "Please select a category to delete.", "OK");
            return;
        }

        bool confirm = await DisplayAlert("Confirm Delete", $"Are you sure you want to delete {_selectedCategory.Name}?", "Yes", "No");
        if (confirm)
        {
            try
            {
                await App.CategoryRepo.DeleteCategoryAsync(_selectedCategory.Id);
                await DisplayAlert("Success", "Category deleted successfully.", "OK");
                LoadCategoriesAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to delete category: {ex.Message}", "OK");
            }
        }
    }

    private void OnCategorySelected(object sender, SelectionChangedEventArgs e)
    {
        _selectedCategory = e.CurrentSelection.FirstOrDefault() as Category;
    }
}
