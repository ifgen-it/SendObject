using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SendObjectClient
{
    [Serializable]
    public class PersonMessage
    {
        public int id;
        public string userName;
        public string message;
        public List<string> tags;
        public byte[] data;

        public PersonMessage()
        {
            id = 0;
            userName = null;
            message = null;
            tags = null;
            data = null;
        }

        public PersonMessage(int ID, string UserName, string Message, List<string> Tags, byte[] Data)
        {
            id = ID;
            userName = UserName;
            message = Message;
            tags = Tags;
            data = Data;
        }

        public override string ToString()
        {
            string res = "";
            res += "id: " + id + "\n";
            res += "name: " + userName + "\n";
            res += "message: " + message + "\n";
            res += "tags: ";
            for (int i = 0; i < tags.Count; i++)
            {
                res += "#" + tags[i] + " ";
            }
            return res;
        }
    }

}
