﻿using System;
using System.Collections.Generic;
using MessageBoard.Api.Models;
using MessageBoard.Api.Models.Requests;
using MessageBoard.Api.Stores;

namespace MessageBoard.Api.Services
{
    public interface IMessagesService
    {
        MessageModel Create(CreateMessageRequest request);
        IEnumerable<MessageModel> GetUserMessages(string userId);
    }

    public class MessagesService : IMessagesService
    {
        private readonly IMessagesStore _messagesStore;

        public MessagesService(IMessagesStore messagesStore)
        {
            _messagesStore = messagesStore;
        }

        public MessageModel Create(CreateMessageRequest request)
        {
            var messageId = Guid.NewGuid().ToString();
            var createdDate = DateTime.UtcNow;

            var messageModel = new MessageModel(messageId, request.UserId, request.Message, createdDate);
            _messagesStore.Store(messageModel);
            return messageModel;
        }

        public IEnumerable<MessageModel> GetUserMessages(string userId)
        {
            return _messagesStore.GetAll(userId);
        }
    }
}
