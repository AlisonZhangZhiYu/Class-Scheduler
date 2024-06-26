﻿using Class_Scheduler.Common.Models;
using Class_Scheduler.Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Scheduler.ViewModels.DialogViewModels
{
    public class AddTeachersDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IRepositoryService repositoryService;

        public string Title => "Add teacher data";

        public event Action<IDialogResult> RequestClose;

        public Teacher Teacher { get; set; }

        public DelegateCommand SubmitCommand { get; private set; }

        public DelegateCommand CancelCommand { get; private set; }

        public string[] SubjectAvailable { get; set; }

        public IRepositoryService RepositoryService { get { return repositoryService; } }
        public string UnavailableTimeSlotString { get; set; }

        public AddTeachersDialogViewModel(IRepositoryService repositoryService) 
        {
            this.repositoryService = repositoryService;

            SubmitCommand = new DelegateCommand(SubmitData);
            CancelCommand = new DelegateCommand(Cancel);
            Teacher = new Teacher();

            SubjectAvailable = repositoryService.SubjectAvailable.ToArray();
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }

        private void SubmitData()
        {

           
            if (UnavailableTimeSlotString != string.Empty && UnavailableTimeSlotString != null)
            {
                Teacher.UnavailableTimeSlotString = UnavailableTimeSlotString;
                string[] slots = UnavailableTimeSlotString.Split(',');
                for (int i = 0; i < slots.Length; i++)
                {
                    if (int.TryParse(slots[i], out int id))
                        Teacher.UnavailableTimeSlotId.Add(id);
                    else
                        System.Diagnostics.Debug.WriteLine("Teacher slot addition error: Format Incorrect!");
                }
            }

            DialogParameters keys = new DialogParameters
            {
                { "Teacher", Teacher }
            };
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, keys));
        }

        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }
    }
}
