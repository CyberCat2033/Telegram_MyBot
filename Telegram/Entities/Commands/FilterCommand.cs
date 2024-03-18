using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegramchik;
using Telegramchik.Commands;

namespace Telegram.Commands;

public class FilterCommand : TelegramCommands
{
    private Dictionary<string, Message> FilterDict = new();

    public FilterCommand(string Command, string Description = "") : base(Command, Description)
    {
    }

    public async Task AddFilter(string ReplyMessageText, Message message)
    {
        ReplyMessageText = ReplyMessageText.ToLower();

        await Task.Run(() =>
        {
            if (!FilterDict.TryAdd(ReplyMessageText, message))
            {
                FilterDict[ReplyMessageText] = message;
            }
        });
    }

    public override async Task Execute(Message message, ITelegramBotClient botClient, CancellationToken CT)
    {
        var chatID = message.Chat.Id;
        Message? FilterReplyMessage;
        if (message is { Type: MessageType.Text, Text: not null })
        {

            if (FilterDict.TryGetValue(message.Text, out FilterReplyMessage))
            {
                _ = FilterReplyMessage.Type switch
                {
                    MessageType.Sticker => await botClient.SendStickerAsync(
                        chatID, InputFile.FromFileId(FilterReplyMessage.Sticker.FileId)),

                    MessageType.Text => await botClient.SendTextMessageAsync(chatID, message.Text),

                    MessageType.Photo => await botClient.SendPhotoAsync(
                        chatID, InputFile.FromFileId(message.Photo[0].FileId)),
                    _ => throw new NotImplementedException(),
                };

            }
        }


    }
}
