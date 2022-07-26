﻿using Application;
using Domain;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;
        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }
        
        private bool CheckProfanity(string message)
        {
            string[] profanity = new string[5] { "idiot", "dumb", "booger", "alligator", "monkey" };

            foreach (string word in profanity)
            {
                if (message.Contains(word)) return true;
            }
            return false;
        }

        public bool ValidateMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return false;// "Please type a message to continue."
            if (message.Length > 256)
                return false;// "The message should have a maximum of 256 characters
            if (CheckProfanity(message))
                return false;// "The message contains profanity
            return true;
        }

        public async Task CreateMessageAsync(Message message)
        {
            if (ValidateMessage(message.Text))
                await _context.Messages.AddAsync(message);
            else
                throw new InvalidMessageException();
        }

        public async Task<List<Message>> GetMessagesBetweenTwoUsersAsync(int idUser1, int idUser2)
        {
            var messages = await _context.Messages
                .Where(m => m.IDSender == idUser1 && m.IDReceiver == idUser2 ||
                m.IDSender == idUser2 && m.IDReceiver == idUser1)
                .ToListAsync();

            if (messages == null)
            {
                return null;
            }

            // add 3 hrs to match ro
            messages.Select(message => { message.DateTime = Convert.ToDateTime(message.DateTime).AddHours(3).ToString(); return message; }).ToList();

            // sort by datetime
            messages.Sort((x, y) => DateTime.Compare(Convert.ToDateTime(x.DateTime), Convert.ToDateTime(y.DateTime)));

            return messages;
        }

        public async Task<Message> GetMessageByIdAsync(int id)
        {
            var message = await _context.Messages
                .Where(m => m.ID == id).FirstOrDefaultAsync();

            if (message == null)
            {
                return null;
            }
            return message;
        }

        public async Task<int> GetNumberOfMessagesBetweenTwoUsersAsync(int idUser1, int idUser2)
        {
            return await _context.Messages
                .Where(m => m.IDSender == idUser1 && m.IDReceiver == idUser2 || m.IDSender == idUser2 && m.IDReceiver == idUser1).CountAsync();
        }

        public async Task<List<Message>> GetSomeMessagesFromOffset(int idUser1, int idUser2, int offset, int numberOfMessages)
        {
            var messages = await _context.Messages
                .Where(m => m.IDSender == idUser1 && m.IDReceiver == idUser2 ||
                m.IDSender == idUser2 && m.IDReceiver == idUser1).OrderByDescending(m => m.ID).Skip(offset).Take(numberOfMessages).OrderBy(m => m.ID).ToListAsync();

            if (messages == null)
            {
                return null;
            }
            return messages;
        }
    }
}
