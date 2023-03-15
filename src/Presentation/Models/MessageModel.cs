using Newtonsoft.Json;

namespace Mttechne.UI.Web.Models;

public enum TypeMessage
{
    Info,
    Error
}

public class MessageModel
{
    public TypeMessage Type { get; set; }
    public string Text { get; set; }
    public MessageModel(string message, TypeMessage type = TypeMessage.Info)
    {
        Type = type;
        Text = message;
    }

    public static string Serialize(string message, TypeMessage type = TypeMessage.Info)
    {
        var mensagemModel = new MessageModel(message, type);
        return JsonConvert.SerializeObject(mensagemModel);
    }

    public static MessageModel Deserialize(string message)
    {
        return JsonConvert.DeserializeObject<MessageModel>(message);
    }
}