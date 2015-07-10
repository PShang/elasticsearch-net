﻿using Nest.Resolvers.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Nest
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	[JsonConverter(typeof(ReadAsTypeConverter<ReverseNestedAggregator>))]
	public interface IReverseNestedAggregator : IBucketAggregator
	{
		[JsonProperty("path")]
		FieldName Path { get; set; }
	}

	public class ReverseNestedAggregator : BucketAggregator, IReverseNestedAggregator
	{
		[JsonProperty("path")]
		public FieldName Path { get; set; }
	}

	public class ReverseNestedAggregationDescriptor<T> 
		: BucketAggregatorBaseDescriptor<ReverseNestedAggregationDescriptor<T>,IReverseNestedAggregator, T>
			, IReverseNestedAggregator 
		where T : class
	{
		FieldName IReverseNestedAggregator.Path { get; set; }

		public ReverseNestedAggregationDescriptor<T> Path(string path) => Assign(a => a.Path = path);

		public ReverseNestedAggregationDescriptor<T> Path(Expression<Func<T, object>> path) => Assign(a => a.Path = path);
	}
}