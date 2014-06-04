using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ProjectNMM.Model
{
    static class GameFileFunctions
    {
        static public bool SaveGame(GameData data, string path)
        {
            XmlWriter writer = XmlWriter.Create(path);
            int counter = 0;

            writer.WriteStartDocument();
            writer.WriteStartElement("Game");

            #region write GameData

            writer.WriteStartElement("GameData");
            writer.WriteAttributeString("GameType", data.GameType.ToString());
            writer.WriteAttributeString("MoveIsActive", data.MoveIsActive.ToString());
            writer.WriteAttributeString("InactiveMoveIndex1", data.InactiveMoveIndex1.ToString());
            writer.WriteAttributeString("InactiveMoveIndex2", data.InactiveMoveIndex2.ToString());
            writer.WriteAttributeString("PlayerName1", RemoveSpecialCharacters(data.PlayerName1));
            writer.WriteAttributeString("PlayerName2", RemoveSpecialCharacters(data.PlayerName2));
            writer.WriteAttributeString("StartTime", data.StartTime.ToString());
            writer.WriteAttributeString("EndTime", data.EndTime.ToString());
            writer.WriteAttributeString("GameIsOver", data.GameIsOver.ToString());
            writer.WriteAttributeString("Winner", data.Winner.ToString());
            writer.WriteAttributeString("Description", RemoveSpecialCharacters(data.Description));
            writer.WriteEndElement();

            #endregion

            #region write Playstones

            writer.WriteStartElement("BoardStates");
            foreach (BoardState state in data.BoardStates)
            {
                writer.WriteStartElement("BoardState");
                writer.WriteAttributeString("ActionID", counter.ToString());
                writer.WriteAttributeString("ActivePlayer", state.ActivePlayer.ToString());
                writer.WriteAttributeString("PlaystonesPlayer1", state.PlaystonesPlayer1.ToString());
                writer.WriteAttributeString("PlaystonesPlayer2", state.PlaystonesPlayer2.ToString());
                writer.WriteAttributeString("BeforeLastTurnWasMill", state.BeforeLastTurnWasMill.ToString());

                for (int i = 0; i <= 6; i++)
                {
                    for (int j = 0; j <= 6; j++)
                    {
                        writer.WriteStartElement("PlaystoneState");
                        writer.WriteAttributeString("Index1", i.ToString());
                        writer.WriteAttributeString("Index2", j.ToString());
                        writer.WriteAttributeString("PlaystoneState", state.Playstones[i, j].ToString());
                        writer.WriteEndElement();
                    }
                }

                writer.WriteEndElement();
                counter++;
            }
            writer.WriteEndElement();

            #endregion

            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Flush();
            writer.Close(); 

            return true;
        }

        static public bool LoadGame(GameData data, string path)
        {
            XmlWriter writer = XmlWriter.Create(path);
            int counter = 0;

            writer.WriteStartDocument();
            writer.WriteStartElement("Game");

            #region write GameData

            writer.WriteStartElement("GameData");
            writer.WriteAttributeString("GameType", data.GameType.ToString());
            writer.WriteAttributeString("MoveIsActive", data.MoveIsActive.ToString());
            writer.WriteAttributeString("InactiveMoveIndex1", data.InactiveMoveIndex1.ToString());
            writer.WriteAttributeString("InactiveMoveIndex2", data.InactiveMoveIndex2.ToString());
            writer.WriteAttributeString("PlayerName1", RemoveSpecialCharacters(data.PlayerName1));
            writer.WriteAttributeString("PlayerName2", RemoveSpecialCharacters(data.PlayerName2));
            writer.WriteAttributeString("StartTime", data.StartTime.ToString());
            writer.WriteAttributeString("EndTime", data.EndTime.ToString());
            writer.WriteAttributeString("GameIsOver", data.GameIsOver.ToString());
            writer.WriteAttributeString("Winner", data.Winner.ToString());
            writer.WriteAttributeString("Description", RemoveSpecialCharacters(data.Description));
            writer.WriteEndElement();

            #endregion

            #region write Playstones

            writer.WriteStartElement("BoardStates");
            foreach (BoardState state in data.BoardStates)
            {
                writer.WriteStartElement("BoardState");
                writer.WriteAttributeString("ActionID", counter.ToString());
                writer.WriteAttributeString("ActivePlayer", state.ActivePlayer.ToString());
                writer.WriteAttributeString("PlaystonesPlayer1", state.PlaystonesPlayer1.ToString());
                writer.WriteAttributeString("PlaystonesPlayer2", state.PlaystonesPlayer2.ToString());
                writer.WriteAttributeString("BeforeLastTurnWasMill", state.BeforeLastTurnWasMill.ToString());

                for (int i = 0; i <= 6; i++)
                {
                    for (int j = 0; j <= 6; j++)
                    {
                        writer.WriteStartElement("PlaystoneState");
                        writer.WriteAttributeString("Index1", i.ToString());
                        writer.WriteAttributeString("Index2", j.ToString());
                        writer.WriteAttributeString("PlaystoneState", state.Playstones[i, j].ToString());
                        writer.WriteEndElement();
                    }
                }

                writer.WriteEndElement();
                counter++;
            }
            writer.WriteEndElement();

            #endregion

            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Flush();
            writer.Close();

            return true;
        }

        // http://stackoverflow.com/questions/1120198/most-efficient-way-to-remove-special-characters-from-string
        static private string RemoveSpecialCharacters(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
