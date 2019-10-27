using System.Collections.Generic;
using gRaFFit.Agar.Views;
using Models;

namespace Controllers {
	public class CharactersContainer {

		#region Singleton

		private CharactersContainer() {

		}

		private static CharactersContainer _instance;

		public static CharactersContainer Instance {
			get { return _instance ?? (_instance = new CharactersContainer()); }
		}

		#endregion

		private List<Character> _characters = new List<Character>();
		private List<CharacterView> _characterViews = new List<CharacterView>();

		public List<Character> Characters => _characters;
		public List<CharacterView> CharacterViews => _characterViews;

		public void PutCharacter(Character character, CharacterView characterView) {
			_characters.Add(character);
			_characterViews.Add(characterView);
		}

		public Character GetCharacter(int ID) {
			return _characters.Find(enemy => enemy.ID == ID);
		}

		public CharacterView GetCharacterView(int ID) {
			return _characterViews.Find(character => character.ID == ID);
		}

		public void Clear() {
			_characters.Clear();
			for (int i = 0; i < _characterViews.Count; i++) {
				if (_characterViews[i] is EnemyView) {
					_characterViews[i].Dispose();
				}
			}

			_characterViews.Clear();
		}
	}
}