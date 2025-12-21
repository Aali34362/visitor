namespace Visitor.Core.DesignPatterns.CQRSPattern.Commands;

public interface ICommand { }

public interface ICommand<out T> : ICommand { };
