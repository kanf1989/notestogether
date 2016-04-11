using System;

namespace Mainprogram
{
    class PokerstarsNoteNote
    {
        private string _text;
        private string _playerName;
        private string _label;
        private string _updateDate;

        public PokerstarsNoteNote()
        {
        }

        public PokerstarsNoteNote (string text, string playerName, string label, string updateDate)
        {
            _text = text;
            _playerName = playerName;
            _label = label;
            _updateDate = updateDate;
        }

        public string getText ()
        {
            return this._text;
        }

        public string getPlayerName()
        {
            return this._playerName;
        }

        public string getLabel()
        {
            return this._label;
        }

        public string getUpdateDate()
        {
            return this._updateDate;
        }

        public void setText(string text)
        {
            _text = text;
        }

        public void setPlayerName(string playerName)
        {
            _playerName = playerName;
        }

        public void setLabel(string label)
        {
            _label = label;
        }

        public void setUpdateDate(string updateDate)
        {
            _updateDate = updateDate;
        }

        public bool equals(PokerstarsNoteNote otherPsNote)
        {
            bool equal = false;
            if ((this._text.Equals(otherPsNote.getText())) && (this._playerName.Equals(otherPsNote.getPlayerName())) && (this._label.Equals(otherPsNote.getLabel())) && (this._updateDate.Equals((otherPsNote.getUpdateDate()))))
            {
                equal = true;
            }
            return equal;
        }
    }
}
