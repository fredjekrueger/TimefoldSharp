namespace TimefoldSharp.Examples.EmployeeScheduling.EmployeeScheduling.Domain
{
    internal class ScheduleState
    {
        public long TenantId { get; set; }

        public int PublishLength { get; set; } // In number of days

        public int DraftLength { get; set; } // In number of days

        public DateTime FirstDraftDate { get; set; }

        public DateTime LastHistoricDate { get; set; }


        public bool IsHistoric(DateTime dateTime)
        {
            return dateTime < GetFirstPublishedDate().Date;
        }

        public bool IsDraft(DateTime dateTime)
        {
            return dateTime >= FirstDraftDate.Date;
        }


        public bool IsPublished(DateTime dateTime)
        {
            return !IsHistoric(dateTime) && !IsDraft(dateTime);
        }


        public bool IsHistoric(Shift shift)
        {
            return IsHistoric(shift.Start);
        }


        public bool IsDraft(Shift shift)
        {
            return IsDraft(shift.Start);
        }


        public bool IsPublished(Shift shift)
        {
            return IsPublished(shift.Start);
        }


        public DateTime GetFirstPublishedDate()
        {
            return LastHistoricDate.AddDays(1);
        }

        public DateTime GetFirstUnplannedDate()
        {
            return FirstDraftDate.AddDays(DraftLength);
        }

    }
}
