namespace ChatNet.Domain.Entity;

public class Message
{
    public Message() { }

    public Message(User sender, User reciver, string message) 
    {
        Sender = sender;
        Reciver = reciver;
        TextMessage = message;
    }
    
    public int Id { get; private set; }
    public User Sender { get; private set; }
    public User Reciver { get; private set; }
    public string TextMessage { get; private set; }
}
