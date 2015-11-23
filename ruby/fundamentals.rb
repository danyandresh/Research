puts 'Hello!'

lander_count = 10

result = nil

puts nil.class

puts result.nil?


a = " xyz "

puts a.strip

puts a

puts a.strip!

puts a

def double(val)
  val * 2
end

puts double("abc")

count =10
message = if count > 10
  "bigger than ten"
else
  "smaller than ten"
end

puts message

res = `time /t`

puts res

puts "Using system"
res = system "time /t"
puts res