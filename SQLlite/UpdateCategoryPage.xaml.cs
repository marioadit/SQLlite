using SQLlite.Models;

namespace SQLlite;

public partial class UpdateCategoryPage : ContentPage
{
    public event EventHandler<Category> CategoryUpdated;

    private Category _category;

    public UpdateCategoryPage(Category category)
    {
        InitializeComponent();
        _category = category;

        // Pre-fill the fields with the current category data
        NameEntry.Text = _category.Name;
        DescriptionEntry.Text = _category.Description;
    }

    private async void OnSaveChangesClicked(object sender, EventArgs e)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            await DisplayAlert("Validation", "Please enter a category name.", "OK");
            return;
        }

        // Update the category object
        _category.Name = NameEntry.Text;
        _category.Description = DescriptionEntry.Text;

        // Notify the main page that the category has been updated
        CategoryUpdated?.Invoke(this, _category);

        // Close the update page
        await Navigation.PopModalAsync();
    }
}
