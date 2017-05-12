namespace Common.Models

open System

type User = 
    { Id : int
      EmailAddress : string 
      Name : string }

type Product = 
    { Id : int
      Name : string }