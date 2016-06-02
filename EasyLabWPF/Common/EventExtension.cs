using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace EasyLabWPF.Common
{
    public class EventExtension : MarkupExtension
    {
        private const string ARGS = "$args";
        private const string THIS = "$this";
        private static readonly MethodInfo GetMethod = typeof(EventExtension).GetMethod("HandlerIntern", new[] { typeof(object), typeof(object), typeof(string), typeof(string) });
        public string Command { get; set; }
        public string CommandParameter { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var targetProvider = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

            if (targetProvider == null) throw new InvalidOperationException();

            var targetObject = targetProvider.TargetObject as FrameworkElement;

            if (targetObject == null) throw new InvalidOperationException();

            var memberInfo = targetProvider.TargetProperty as MemberInfo;

            if (memberInfo == null) throw new InvalidOperationException();

            return CreateHandler(memberInfo, Command);
        }

        private object CreateHandler(MemberInfo memberInfo, string cmdName)
        {
            var eventHandlerType = GetEventHandlerType(memberInfo);

            if (eventHandlerType == null) return null;

            var handlerInfo = eventHandlerType.GetMethod("Invoke");
            var method = new DynamicMethod("", handlerInfo.ReturnType, new[]
            {
                handlerInfo.GetParameters()[0].ParameterType,
                handlerInfo.GetParameters()[1].ParameterType
            });

            var gen = method.GetILGenerator();
            gen.Emit(OpCodes.Ldarg, 0);
            gen.Emit(OpCodes.Ldarg, 1);
            gen.Emit(OpCodes.Ldstr, cmdName);

            if (CommandParameter == null)
            {
                gen.Emit(OpCodes.Ldnull);
            }
            else
            {
                gen.Emit(OpCodes.Ldstr, CommandParameter);
            }
            gen.Emit(OpCodes.Call, GetMethod);
            gen.Emit(OpCodes.Ret);

            return method.CreateDelegate(eventHandlerType);
        }

        private Type GetEventHandlerType(MemberInfo memberInfo)
        {
            Type eventHandlerType = null;
            var info = memberInfo as EventInfo;
            if (info != null)
            {
                var eventInfo = info;
                eventHandlerType = eventInfo.EventHandlerType;
            }
            else
            {
                var methodInfo = memberInfo as MethodInfo;
                if (methodInfo != null)
                {
                    var pars = methodInfo.GetParameters();
                    eventHandlerType = pars[1].ParameterType;
                }
            }
            return eventHandlerType;
        }

        public static void HandlerIntern(object sender, object args, string cmdName, string commandParameter)
        {
            var fe = sender as FrameworkElement;
            if (fe != null)
            {
                var cmd = GetCommand(fe, cmdName);
                object commandParam = null;
                if (!string.IsNullOrWhiteSpace(commandParameter))
                {
                    commandParam = GetCommandParameter(fe, args, commandParameter);
                }
                if ((cmd != null) && cmd.CanExecute(commandParam))
                {
                    cmd.Execute(commandParam);
                }
            }
        }

        internal static ICommand GetCommand(FrameworkElement target, string cmdName)
        {
            var vm = FindViewModel(target);
            if (vm == null) return null;


            var vmType = vm.GetType();
            var cmdProp = vmType.GetProperty(cmdName);
            if (cmdProp != null)
            {
                return cmdProp.GetValue(vm) as ICommand;
            }
            Debug.Assert(cmdProp != null, string.Format("EventBinding path error. {0} property not found on {1}", cmdName, vmType));
            return null;
        }

        internal static object GetCommandParameter(FrameworkElement target, object args, string commandParameter)
        {
            object ret;
            var classify = commandParameter.Split('.');
            switch (classify[0])
            {
                case ARGS:
                    ret = args;
                    break;
                case THIS:
                    ret = classify.Length > 1 ? FollowPropertyPath(target, commandParameter.Replace("$this.", ""), target.GetType()) : target;
                    break;
                default:
                    ret = commandParameter;
                    break;
            }
            return ret;
        }

        internal static ObservableObject FindViewModel(FrameworkElement target)
        {
            if (target == null) return null;


            var vm = target.DataContext as ObservableObject;
            if (vm != null) return vm;

            var parent = target.Parent as FrameworkElement;

            return FindViewModel(parent);
        }

        internal static object FollowPropertyPath(object target, string path, Type valueType = null)
        {
            if (target == null) throw new ArgumentNullException("target");
            if (path == null) throw new ArgumentNullException("path");


            var currentType = valueType ?? target.GetType();


            foreach (var propertyName in path.Split('.'))
            {
                var property = currentType.GetProperty(propertyName);
                if (property == null) throw new NullReferenceException("property");

                target = property.GetValue(target);
                currentType = property.PropertyType;
            }
            return target;
        }
    }
}
