using System;
using System.Collections.Generic;

class Song
{
    string name;   // название песни
    string author; // автор песни
    Song prev;     // связь с предыдущей песней в списке

    // Конструктор по умолчанию (исправляет ошибку при создании new Song())
    public Song()
    {
    }

    // 1. Конструктор с названием и автором (prev = null)
    public Song(string songName, string songAuthor)
    {
        name = songName;
        author = songAuthor;
        prev = null;
    }

    // 2. Конструктор с названием, автором и предыдущей песней
    public Song(string songName, string songAuthor, Song prevSong)
    {
        name = songName;
        author = songAuthor;
        prev = prevSong;
    }

    public void SetName(string songName)
    {
        name = songName;
    }

    public void SetAuthor(string songAuthor)
    {
        author = songAuthor;
    }

    public void SetPrev(Song prevSong)
    {
        prev = prevSong;
    }

    public string Title()
    {
        return name + " — " + author;
    }

    public override bool Equals(object d)
    {
        Song otherSong = d as Song;
        if (otherSong == null)
        {
            return false;
        }
        return this.name == otherSong.name && this.author == otherSong.author;
    }
}

class Program
{
    static void Main()
    {
        List<Song> playlist = new List<Song>();

        // Используем новые конструкторы
        Song song1 = new Song("Mr. Tambourine Man", "Bob Dylan");
        playlist.Add(song1);

        Song song2 = new Song("I Walk The Line", "Johnny Cash", song1);
        playlist.Add(song2);

        Song song3 = new Song("Farewell Angelina", "Joan Baez", song2);
        playlist.Add(song3);

        Song song4 = new Song("If I Can Dream", "Elvis Presley", song3);
        playlist.Add(song4);

        // Создание объекта из условия задачи (работает благодаря конструктору по умолчанию)
        Song mySong = new Song();
        mySong.SetName("Mr. Tambourine Man");
        mySong.SetAuthor("Bob Dylan");
        playlist.Add(mySong);

        Console.WriteLine("Список песен:");
        foreach (Song s in playlist)
        {
            Console.WriteLine(s.Title());
        }

        // Исправленное сравнение первой (индекс 0) и второй (индекс 1) песни
        Console.WriteLine("\nСравнение первой и второй песни:");
        if (playlist[0].Equals(playlist[1]))
        {
            Console.WriteLine("Первая и вторая песни одинаковые.");
        }
        else
        {
            Console.WriteLine("Первая и вторая песни разные.");
        }

        Console.ReadLine();
    }
}
