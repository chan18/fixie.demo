﻿namespace ContactList.Tests.Features.Contact
{
    using System.Linq;
    using System.Threading.Tasks;
    using ContactList.Features.Contact;
    using ContactList.Model;
    using Shouldly;
    using static Testing;

    public class ContactIndexTests
    {
        public async Task ShouldGetAllContactsSortedByName()
        {
            var ben = new AddContact.Command
            {
                Email = "ben@example.com",
                Name = "Ben",
                PhoneNumber = "555-123-0001"
            };

            var cathy = new AddContact.Command
            {
                Email = "cathy@example.com",
                Name = "Cathy",
                PhoneNumber = "555-123-0002"
            };

            var abe = new AddContact.Command
            {
                Email = "abe@example.com",
                Name = "Abe",
                PhoneNumber = "555-123-0003"
            };

            var benId = (await Send(ben)).ContactId;
            var cathyId = (await Send(cathy)).ContactId;
            var abeId = (await Send(abe)).ContactId;

            var expectedIds = new[] { benId, cathyId, abeId };

            var query = new ContactIndex.Query();

            var result = await Send(query);

            result.Length.ShouldBe(Count<Contact>());

            result
                .Where(x => expectedIds.Contains(x.Id))
                .ShouldMatch(
                    new ContactIndex.ViewModel
                    {
                        Id = abeId,
                        Name = "Abe",
                        Email = "abe@example.com",
                        PhoneNumber = "555-123-0003"
                    },
                    new ContactIndex.ViewModel
                    {
                        Id = benId,
                        Name = "Ben",
                        Email = "ben@example.com",
                        PhoneNumber = "555-123-0001"
                    },
                    new ContactIndex.ViewModel
                    {
                        Id = cathyId,
                        Name = "Cathy",
                        Email = "cathy@example.com",
                        PhoneNumber = "555-123-0002"
                    }
                );
        }
    }
}