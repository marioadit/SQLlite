using SQLlite.Models;
using SQLite;

namespace SQLlite
{
    public class CategoryRepository
    {
        private string _dbPath;
        private SQLiteAsyncConnection conn;

        public string StatusMessage { get; set; }

        public CategoryRepository(string dbPath)
        {
            _dbPath = dbPath;
        }

        private async Task Init()
        {
            if (conn != null)
                return;

            conn = new SQLiteAsyncConnection(_dbPath);
            await conn.CreateTableAsync<Category>();
        }

        // Method to retrieve all categories
        public async Task<List<Category>> GetCategoriesAsync()
        {
            try
            {
                await Init();
                return await conn.Table<Category>().ToListAsync();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to retrieve data: {ex.Message}";
                return new List<Category>();
            }
        }

        // Method to add a new category
        public async Task AddCategoryAsync(Category category)
        {
            try
            {
                await Init();

                // Validate the data
                if (string.IsNullOrEmpty(category.Name))
                    throw new Exception("Category name is required");

                var existingCategory = await conn.Table<Category>()
                    .Where(c => c.Name == category.Name)
                    .FirstOrDefaultAsync();

                if (existingCategory != null)
                    throw new Exception("Category name already exists");

                await conn.InsertAsync(category);
                StatusMessage = $"Category '{category.Name}' added successfully.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to add category: {ex.Message}";
            }
        }

        // Method to update a category
        public async Task UpdateCategoryAsync(Category category)
        {
            try
            {
                await Init();

                if (category.Id == 0)
                    throw new Exception("Invalid category ID");

                var existingCategory = await conn.Table<Category>()
                    .Where(c => c.Id == category.Id)
                    .FirstOrDefaultAsync();

                if (existingCategory == null)
                    throw new Exception("Category not found");

                existingCategory.Name = category.Name;
                existingCategory.Description = category.Description;

                await conn.UpdateAsync(existingCategory);
                StatusMessage = $"Category '{category.Name}' updated successfully.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to update category: {ex.Message}";
            }
        }

        // Method to delete a category
        public async Task DeleteCategoryAsync(int id)
        {
            try
            {
                await Init();

                var categoryToDelete = await conn.Table<Category>()
                    .Where(c => c.Id == id)
                    .FirstOrDefaultAsync();

                if (categoryToDelete == null)
                    throw new Exception("Category not found");

                await conn.DeleteAsync(categoryToDelete);
                StatusMessage = $"Category '{categoryToDelete.Name}' deleted successfully.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Failed to delete category: {ex.Message}";
            }
        }
    }
}
