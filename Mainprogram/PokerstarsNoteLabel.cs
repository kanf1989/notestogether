using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mainprogram
{
    class PokerstarsNoteLabel
    {
        private string _id;
        private string _color;
        private string _labelName;

        public PokerstarsNoteLabel()
        {

        }

        public PokerstarsNoteLabel(string labelAsText)
        {
            extractLabelFromText(labelAsText);
        }
        
        public PokerstarsNoteLabel(String id, String color, String labelName)
        {
            _id = id;
            _color = color;
            _labelName = labelName;
        }
        
        public string getId()
        {
            return this._id;
        }

        public string getColor()
        {
            return this._color;
        }


        public string getLabelName()
        {
            return this._labelName;
        }

        public void setId(string id)
        {
            this._id = id;
        }

        public void setColor(string color)
        {
            this._color = color;
        }


        public void setLabelName(string text)
        {
            this._labelName = text;
        }

        // Extracts the informations of the label-Xml-Text an saves them to the objekt
        // @param labelAsText -  <nodeName>...</nodeName>
        //
        private void extractLabelFromText(string labelAsText)
        {          
            int idLength = -1;
            int colorLength = 0;
                        
            if (labelAsText.IndexOf("id=") != -1)
            {
                idLength = countAttributeLength(labelAsText, "id");                
                this.setId(labelAsText.Substring((labelAsText.IndexOf("id=") + 4), idLength));
            }

            if (labelAsText.IndexOf("color=") != - 1)
            {
                //einlesen der Farbe des labels
                colorLength = countAttributeLength(labelAsText, "color");
                this.setColor(labelAsText.Substring(labelAsText.IndexOf("color=", 0) + 7, colorLength));
            }

            this.setLabelName(labelAsText.Substring(labelAsText.IndexOf(">") + 1, (labelAsText.Length - labelAsText.IndexOf(">") - 9))); //einlesen der Beschreibung des Labels       
        }

        private int countAttributeLength(string labelAsText, string attribute)
        {
            string tempChar;
            int quotationCounter = 0;
            int workingPos = 0, attributeLength = -1;

            //einlesen der id des Labels
            if (attribute.Equals("id"))
            {
                workingPos = labelAsText.IndexOf("id") + 2;
            } else if (attribute.Equals("color"))
            {
                workingPos = labelAsText.IndexOf("color") + 5;
            } else
            {
                //Das kann nicht sein -> todo Throw Exception
            }

            while (quotationCounter != 2)
            {
                tempChar = labelAsText.Substring(workingPos, 1);
                if (tempChar == "\"") quotationCounter++;
                else attributeLength++;
                workingPos++;
            }

            return attributeLength;
        }
    }
}
