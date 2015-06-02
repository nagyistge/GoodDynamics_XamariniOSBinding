﻿using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;

namespace GDSQLite {
	public static class SQLite3 {
		public enum Result : int {
			OK = 0,
			Error = 1,
			Internal = 2,
			Perm = 3,
			Abort = 4,
			Busy = 5,
			Locked = 6,
			NoMem = 7,
			ReadOnly = 8,
			Interrupt = 9,
			IOError = 10,
			Corrupt = 11,
			NotFound = 12,
			TooBig = 18,
			Constraint = 19,
			Row = 100,
			Done = 101
		}

		public enum ConfigOption : int {
			SingleThread = 1,
			MultiThread = 2,
			Serialized = 3
		}
				
		[DllImport("__Internal", EntryPoint = "sqlite3_open")]
		public static extern Result Open (string filename, out IntPtr db);

		[DllImport("__Internal", EntryPoint = "sqlite3_close")]
		public static extern Result Close (IntPtr db);

		[DllImport("__Internal", EntryPoint = "sqlite3_config")]
		public static extern Result Config (ConfigOption option);

		[DllImport("__Internal", EntryPoint = "sqlite3_busy_timeout")]
		public static extern Result BusyTimeout (IntPtr db, int milliseconds);

		[DllImport("__Internal", EntryPoint = "sqlite3_changes")]
		public static extern int Changes (IntPtr db);

		[DllImport("__Internal", EntryPoint = "sqlite3_prepare_v2")]
		public static extern Result Prepare2 (IntPtr db, string sql, int numBytes, out IntPtr stmt, IntPtr pzTail);

		[DllImport("__Internal", EntryPoint = "sqlite3_prepare_v2")]
		public static extern Result Prepare2 (IntPtr db, string sql, int numBytes, out IntPtr stmt, out IntPtr pzTail);

		[DllImport("__Internal", EntryPoint= "sqlite3_prepare_v2")]
		public static extern Result Prepare2 (IntPtr db, IntPtr sql, int numBytes, out IntPtr stmt, out IntPtr pzTail);

		public static IntPtr Prepare2(IntPtr db, string query)
		{
			IntPtr statement = IntPtr.Zero;
			IntPtr errorPtr = IntPtr.Zero;
			IntPtr queryPtr = System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi (query); 

			var sqliteResult = SQLite3.Prepare2 (db, queryPtr, query.Length, out statement, out errorPtr);


			return statement;
		}

		[DllImport("__Internal", EntryPoint = "sqlite3_step")]
		public static extern Result Step (IntPtr stmt);

		[DllImport("__Internal", EntryPoint = "sqlite3_reset")]
		public static extern Result Reset (IntPtr stmt);

		[DllImport("__Internal", EntryPoint = "sqlite3_finalize")]
		public static extern Result Finalize (IntPtr stmt);

		[DllImport("__Internal", EntryPoint = "sqlite3_last_insert_rowid")]
		public static extern long LastInsertRowid (IntPtr db);

		[DllImport("__Internal", EntryPoint = "sqlite3_errmsg16")]
		public static extern IntPtr Errmsg (IntPtr db);

		[DllImport("__Internal", EntryPoint = "sqlite3_bind_parameter_index")]
		public static extern int BindParameterIndex (IntPtr stmt, string name);

		[DllImport("__Internal", EntryPoint = "sqlite3_bind_null")]
		public static extern int BindNull (IntPtr stmt, int index);

		[DllImport("__Internal", EntryPoint = "sqlite3_bind_int")]
		public static extern int BindInt (IntPtr stmt, int index, int val);

		[DllImport("__Internal", EntryPoint = "sqlite3_bind_int64")]
		public static extern int BindInt64 (IntPtr stmt, int index, long val);

		[DllImport("__Internal", EntryPoint = "sqlite3_bind_double")]
		public static extern int BindDouble (IntPtr stmt, int index, double val);

		[DllImport("__Internal", EntryPoint = "sqlite3_bind_text")]
		public static extern int BindText (IntPtr stmt, int index, string val, int n, IntPtr free);

		[DllImport("__Internal", EntryPoint = "sqlite3_bind_text")]
		public static extern int BindText (IntPtr stmt, int index, IntPtr val, int n, IntPtr free);

		[DllImport("__Internal", EntryPoint = "sqlite3_bind_blob")]
		public static extern int BindBlob (IntPtr stmt, int index, byte[] val, int n, IntPtr free);

		[DllImport("__Internal", EntryPoint = "sqlite3_column_count")]
		public static extern int ColumnCount (IntPtr stmt);

		[DllImport("__Internal", EntryPoint = "sqlite3_column_name")]
		public static extern IntPtr ColumnName (IntPtr stmt, int index);

		[DllImport("__Internal", EntryPoint = "sqlite3_column_name16")]
		private static extern IntPtr ColumnName16Internal (IntPtr stmt, int index);
		public static string ColumnName16(IntPtr stmt, int index)
		{
		return Marshal.PtrToStringUni(ColumnName16Internal(stmt, index));
		}

		[DllImport("__Internal", EntryPoint = "sqlite3_column_type")]
		public static extern ColType ColumnType (IntPtr stmt, int index);

		[DllImport("__Internal", EntryPoint = "sqlite3_column_int")]
		public static extern int ColumnInt (IntPtr stmt, int index);

		[DllImport("__Internal", EntryPoint = "sqlite3_column_int64")]
		public static extern long ColumnInt64 (IntPtr stmt, int index);

		[DllImport("__Internal", EntryPoint = "sqlite3_column_double")]
		public static extern double ColumnDouble (IntPtr stmt, int index);

		[DllImport("__Internal", EntryPoint = "sqlite3_column_text")]
		public static extern IntPtr ColumnText (IntPtr stmt, int index);

		[DllImport("__Internal", EntryPoint = "sqlite3_column_text16")]
		public static extern IntPtr ColumnText16 (IntPtr stmt, int index);

		[DllImport("__Internal", EntryPoint = "sqlite3_column_blob")]
		public static extern IntPtr ColumnBlob (IntPtr stmt, int index);

		[DllImport("__Internal", EntryPoint = "sqlite3_column_bytes")]
		public static extern int ColumnBytes (IntPtr stmt, int index);

		public static string ColumnString (IntPtr stmt, int index)
		{
			return Marshal.PtrToStringUni (SQLite3.ColumnText16 (stmt, index));
		}

		public static byte[] ColumnByteArray (IntPtr stmt, int index)
		{
			int length = ColumnBytes (stmt, index);
			byte[] result = new byte[length];
			if (length > 0)
				Marshal.Copy (ColumnBlob (stmt, index), result, 0, length);
			return result;
		}
			
		public enum ColType : int {
			Integer = 1,
			Float = 2,
			Text = 3,
			Blob = 4,
			Null = 5
		}
	}

}

