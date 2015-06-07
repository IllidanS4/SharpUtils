﻿using System;
using System.Runtime.InteropServices;
using IllidanS4.SharpUtils.Accessing;
using IllidanS4.SharpUtils.Metadata;
using IllidanS4.SharpUtils.Unsafe;

namespace IllidanS4.SharpUtils.Interop
{
	/// <summary>
	/// Generic equivalent of <see cref="System.IntPtr"/>.
	/// </summary>
	[Unsafe]
	public unsafe struct Pointer<T> : IPointer, IReadAccessor<T>, IWriteAccessor<T>, ITypedReference where T : struct
	{
		void* ptr;
		private static readonly Type ptrType = TypeOf<T>.TypeID;
		
		public Pointer(IntPtr pointer)
		{
			ptr = (void*)pointer;
		}
		
		[CLSCompliant(false)]
		public Pointer(void* pointer)
		{
			ptr = pointer;
		}
		
		[Obsolete("Converting managed pointer to unmanaged needs pinning, use UnsafeTools.GetPointer.", true)]
 		public Pointer(ref T value) : this(__makeref(value))
		{
			
		}
		
		[CLSCompliant(false)]
		public Pointer(TypedReference reference)
		{
			ptr = (void*)reference.ToPointer();
		}
		
		public IntPtr ToIntPtr()
		{
			return (IntPtr)ptr;
		}
		
		[CLSCompliant(false)]
		public void* ToPointer()
		{
			return (void*)ptr;
		}
		
		public Type Type{
			get{
				return ptrType.MakePointerType();
			}
		}
		
		public bool IsNull{
			get{
				return ptr == default(void*);
			}
		}
		
		[CLSCompliant(false)]
		public void GetReference([Out]TypedReference* tr)
		{
			var tptr = (IntPtr*)tr;
			tptr[0] = ToIntPtr();
			tptr[1] = ptrType.TypeHandle.Value;
		}
		
		[Boxed(typeof(TypedReference))]
		public ValueType Reference{
			get{
				TypedReference tr;
				GetReference(&tr);
				return UnsafeTools.Box(tr);
			}
		}
		
		
		
		public T Value{
			get{
				TypedReference tr;
				GetReference(&tr);
				return __refvalue(tr, T);
			}
			set{
				TypedReference tr;
				GetReference(&tr);
				__refvalue(tr, T) = value;
			}
		}
		
		T IReadAccessor<T>.Item{
			get{
				return Value;
			}
		}
		
		T IWriteAccessor<T>.Item{
			set{
				Value = value;
			}
		}
		
		Type IStorageAccessor.ItemType{
			get{
				return ptrType;
			}
		}
		
		object IReadAccessor.Item{
			get{
				return Value;
			}
		}
		
		object IWriteAccessor.Item{
			set{
				Value = (T)value;
			}
		}
	}
}