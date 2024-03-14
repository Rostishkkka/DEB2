using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
using WpfApp2.Models;

namespace WpfApp2
{
	public partial class MainWindow : Window
	{
		int idPet;
		public MainWindow()
		{
			InitializeComponent();
			LoadPets();
			LoadSpecies();
			idPet = new int();
		}

		public void LoadPets()
		{
			petsGrid.ItemsSource = DBUtil.db.Animals.Select(s => new
			{
				s.ID,
				s.name_pet,
				s.specie_petNavigation.name_specie
			}).ToList();
		}

		public void LoadSpecies()
		{
			speciePetBox.ItemsSource = DBUtil.db.SpeciesAnimals.Select(s => s.name_specie).ToList();
		}

		public void AddPet(object sender, EventArgs e)
		{
			Animal animal = new Animal();

			animal.name_pet = namePetsBox.Text;
			animal.specie_pet = DBUtil.db.SpeciesAnimals.Where(w => w.name_specie == speciePetBox.Text).Select(s => s.ID).First();

			DBUtil.db.Animals.Add(animal);
			DBUtil.db.SaveChanges();

			LoadPets();
		}

		public void SaveChangeClick(object sender, EventArgs e)
		{
			if (idPet == 0)
			{
				MessageBox.Show("Не выбран питомец", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			Animal animal = DBUtil.db.Animals.FirstOrDefault(f => f.ID == idPet);

			animal.name_pet = namePetChange.Text;

			DBUtil.db.SaveChanges();
			LoadPets();
		}

		public void DeleteClick(object sender, EventArgs e)
		{
			if(idPet == 0)
			{
				MessageBox.Show("Не выбран питомец", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			Animal animal = DBUtil.db.Animals.FirstOrDefault(f => f.ID == idPet);

			DBUtil.db.Animals.Remove(animal);
			DBUtil.db.SaveChanges();

			LoadPets();
		}

		public void SelectedPet(object sender, SelectionChangedEventArgs e)
		{
			if(petsGrid.SelectedItem != null)
			{
				List<string> pets = new List<string>();

				foreach (var column in petsGrid.Columns)
				{
					if(column is DataGridBoundColumn bound)
					{
						var binding = bound.Binding as Binding;
						if(binding != null)
						{
							string propertyName = binding.Path.Path;
							pets.Add(petsGrid.SelectedItem.GetType().GetProperty(propertyName)?.GetValue(petsGrid.SelectedItem, null).ToString());
						}
					}
				}

				idPet = Convert.ToInt32(pets[0]);
				namePetChange.Text = pets[1];
			}
		}
	}
}
