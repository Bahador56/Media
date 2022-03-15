﻿using System.ComponentModel.DataAnnotations;

namespace Media.Models.Entity
{
    public class File
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreateAt { get; set; }
        public string? Location { get; set; }
        public string Format { get; set; }
        public string Path { get; set; }
        public int Size { get; set; }
        public string Description { get; set; }
        public string Caption { get; set; }
        public int FileTypeId { get; set; }
        public virtual FileType FileType { get; set; }
        public int FolderId { get; set; }
        public virtual Folder Folder  { get; set; }

    }
    public class FileType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<File> Files { get; set; }
    }

    public class Folder
    {
        public int Id { get; set; }
        [Required (ErrorMessage ="ورود عنوان الزامی است")]
        [Range(5,50,ErrorMessage =" حداقل {1} کاراکتر حداکثر {2} کاراکتر")]
        public string Titel { get; set; }
        [Required(ErrorMessage = "ورود توضیحات الزامی است")]
        public string Description { get; set; }
        public DateTime CreateAt { get; set; }
        public Guid CreatorId { get; set; }
        public int? ParentId { get; set; }
        public virtual Folder? Parent { get; set; }
        public virtual List<File>? Files { get; set; }
    }
}